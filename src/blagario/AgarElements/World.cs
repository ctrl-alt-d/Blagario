using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blagario.elements
{

    public class World : AgarElement
    {
        public List<AgarElement> Elements;
        public const double WorldSize = 1000; 
        public const long MaxMass = 60 * 1000;
        public const long MaxViruses = 100;        
        public const long MaxPellets = 1000;        

        public override string CssStyle( Player c ) =>$@"
            top: {c.YGame2Physics(0)}px ;
            left: {c.XGame2Physics(0)}px;
            width: {((long)(X * c.Zoom)).ToString()}px ;
            height: {((long)(Y * c.Zoom)).ToString()}px ; 
            "; 

        public World(Universe universe)
        {
            this.X = WorldSize;
            this.Y = WorldSize;
            Elements = new List<AgarElement>();
            Universe = universe;
        }

        public long TotalMass => Elements.Sum( x=> x.Mass );
        public IEnumerable<Virus> Viruses => Elements.Where(x => x.ElementType == ElementType.Virus ).Select(x=>x as Virus);
        public IEnumerable<Pellet> Pellets => Elements.Where(x => x.ElementType == ElementType.Pellet ).Select(x=>x as Pellet);

        public IEnumerable<Cell> Leaderboard => Elements.Where(x => x.ElementType == ElementType.Cell ).Select(x=> x as Cell).OrderByDescending(x => x._Mass ).Take(10);


        public event EventHandler OnTicReached;

        protected virtual void OnTic(EventArgs e)
        {
            EventHandler handler = OnTicReached;
            handler?.Invoke(this, e);
        }
        public override async Task Tic(int fpsTicNum)
        {
            List<AgarElement> currentElements;

            lock(this.Elements) currentElements = this.Elements.OrderBy(e=>e._Mass).ToList();
            foreach (var e in currentElements) await e.Tic(fpsTicNum);

            if (fpsTicNum % (TimedHostedService.fps/3) == 0)  // 3 times per second
            {
                ManageCollitions(currentElements);
            }

            if ( (fpsTicNum+5) % TimedHostedService.fps == 0) // 1 time per second
            {
                currentElements = FillWorld();
            }

            await base.Tic(fpsTicNum);

            OnTic( EventArgs.Empty );
        }

        private List<AgarElement> FillWorld()
        {
            List<AgarElement> currentElements;
            lock (this.Elements) currentElements = this.Elements.ToList();
            CheckIfWoldNedsMoreViruses(currentElements);
            CheckIfWoldNedsMorePellets(currentElements);
            return currentElements;
        }

        private void ManageCollitions(List<AgarElement> currentElements)
        {
            var collisions = LocateCollisions(currentElements);
            lock (this.Elements) ResolveCollitions(collisions);
        }

        private List<( AgarElement eater, List<AgarElement> eateds)> LocateCollisions(List<AgarElement> currentElements)
        {
            List<( AgarElement eater, List<AgarElement> eateds)> collision = new List<( AgarElement eater, List<AgarElement> eateds)>();

            var cells = 
                currentElements
                .Select( (e,i) => new {e,i} )
                .Where(x=>x.e.ElementType == ElementType.Cell)
                .ToList();

            foreach( var currentElement in cells )
            {
                var p = currentElements
                .Take( currentElement.i )
                .Where( otherElement => 
                        ElementsHelper.CanOneElementEatsOtherOneByMass( currentElement.e, otherElement ) &&
                        ElementsHelper.CanOneElementEatsOtherOneByDistance( currentElement.e, otherElement ) )
                .ToList<AgarElement>();

                if (p.Any())
                {
                    collision.Add( (currentElement.e, p) );
                }
            }
            return collision;
        }

        private void ResolveCollitions(List<( AgarElement eater, List<AgarElement> eateds)> collisions)
        {
            foreach(var (eater, eateds) in collisions)
                foreach(var eated in eateds)
                {
                    if (eated._Mass == 0) continue;
                    var t = (eater.ElementType, eated.ElementType );
                    var _ = 
                        t == (ElementType.Cell, ElementType.Pellet)?ResolveEatElements( eater as Cell, eated as Pellet):
                        t == (ElementType.Cell, ElementType.Cell)?ResolveEatElements( eater as Cell, eated as Cell):
                        t == (ElementType.Cell, ElementType.Virus)?ResolveEatElements( eater as Cell, eated as Virus):
                        0;
                }
            var removed = this.Elements.RemoveAll(e=>e._Mass == 0);
        }

        private int ResolveEatElements(Cell eater, Pellet eated)
        {
            eater._EatedMass += eated._Mass;
            eated._Mass = 0;
            return 1;
        }
        private int ResolveEatElements(Cell eater, Cell eated)
        {
            eater._EatedMass += eated._Mass;
            eated._Mass = 0;
            return 1;
        }
        private int ResolveEatElements(Cell eater, Virus eated)
        {
            eater._EatedMass += eated._Mass;
            eated._Mass = 0;
            return 1;
        }

        
        private void CheckIfWoldNedsMorePellets(List<AgarElement> currentElements)
        {
            var nPellets = currentElements.Where(x=>x.ElementType == ElementType.Pellet).Count();
            var mass = currentElements.Sum(e=>e.Mass);
            if ( nPellets < MaxPellets && mass < MaxMass )
            lock(this.Elements)
            while( nPellets < MaxPellets && mass < MaxMass )
            {
                var e = Pellet.CreatePellet(this.Universe);        
                mass += e.Mass;        
                nPellets++;
            }      
        }

        private void CheckIfWoldNedsMoreViruses(List<AgarElement> currentElements)
        {
            var nViruses = currentElements.Where(x=>x.ElementType == ElementType.Pellet).Count();
            var mass = currentElements.Sum(e=>e.Mass);

            if (nViruses < MaxViruses && mass < MaxMass)
            lock(this.Elements)
            while( nViruses < MaxViruses && mass < MaxMass )
            {
                var e = Virus.CreateVirus(this.Universe);
                mass += e.Mass;        
                nViruses++;                
            }
        }
    }
}
