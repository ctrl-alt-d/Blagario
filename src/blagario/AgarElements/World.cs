using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blagario.elements
{

    public class World : AgarElement
    {
        public List<AgarElement> Elements;
        public const long MaxMass = 60 * 1000;
        public const long MaxViruses = 100;        
        public const long MaxPellets = 10000;        

        public override string CssStyle( Player c ) =>$@"
            top: {c.YGame2Physics(0)}px ;
            left: {c.XGame2Physics(0)}px;
            width: {(X * c.Zoom).ToString()}px ;
            height: {(Y * c.Zoom).ToString()}px ; 
            "; 

        public World(Universe universe)
        {
            this.X = 1000;
            this.Y = 1000;
            Elements = new List<AgarElement>();
            Universe = universe;
        }

        public long TotalMass => Elements.Sum( x=> x.Mass );
        public IEnumerable<Virus> Viruses => Elements.Where(x => x.ElementType == ElementType.Virus ).Select(x=>x as Virus);
        public IEnumerable<Pellet> Pellets => Elements.Where(x => x.ElementType == ElementType.Pellet ).Select(x=>x as Pellet);


        public event EventHandler OnTicReached;

        protected virtual void OnTic(EventArgs e)
        {
            EventHandler handler = OnTicReached;
            handler?.Invoke(this, e);
        }
        public override async Task Tic()
        {
            List<AgarElement> currentElements;

            lock(this.Elements) currentElements = this.Elements.OrderByDescending(e=>e._Mass).ToList();
            foreach (var e in currentElements) await e.Tic();

            var collisions = LocateCollisions(currentElements);
            lock(this.Elements) ResolveCollitions(collisions);

            lock(this.Elements) currentElements = this.Elements.ToList();
            CheckIfWoldNedsMoreViruses(currentElements);            
            CheckIfWoldNedsMorePellets(currentElements);            


            await base.Tic();

            OnTic( EventArgs.Empty );
        }

        private void ResolveCollitions(List<( AgarElement eater, List<AgarElement> eateds)> collisions)
        {
            foreach(var c in collisions)
            {
                foreach(var eated in c.eateds)
                {
                    if (c.eater._Mass * 0.9 > eated._Mass && eated._Mass != 0)
                    {
                        ResolveEat( c.eater, eated);
                    }
                }
            }
            var removed = this.Elements.RemoveAll(e=>e._Mass == 0);

        }
        private void ResolveEat(AgarElement eater, AgarElement eated)
        {
            // Combinations:
            ResolveEatElements( eater as Cell, eated as Pellet);
            ResolveEatElements( eater as Cell, eated as Cell);
            ResolveEatElements( eater as Cell, eated as Virus);            
        }

        private void ResolveEatElements(Cell eater, Pellet eated)
        {
            if (eater == null || eated == null) return;
            eater._Mass += eated._Mass;
            eated._Mass = 0;
        }
        private void ResolveEatElements(Cell eater, Cell eated)
        {
        }
        private void ResolveEatElements(Cell eater, Virus eated)
        {
        }
        
        private List<( AgarElement eater, List<AgarElement> eateds)> LocateCollisions(List<AgarElement> currentElements)
        {
            List<( AgarElement eater, List<AgarElement> eateds)> collision = new List<( AgarElement eater, List<AgarElement> eateds)>();

            var elements = currentElements.Select( (e,i) => new {e,i} ).ToList();
            var cells = elements.Where(x=>x.e.ElementType == ElementType.Cell).ToList();

            foreach( var currentElement in cells )
            {
                var ninetingPercentMass = currentElement.e._Mass * 0.9;

                var p = currentElements
                .Skip( currentElement.i )
                .Where( otherElement => ninetingPercentMass> otherElement._Mass)
                .Where( otherElement => Math.Abs(otherElement.X - currentElement.e.X) < ( otherElement.Radius + currentElement.e.Radius ) )
                .Where( otherElement => Math.Abs(otherElement.Y - currentElement.e.Y) < ( otherElement.Radius + currentElement.e.Radius ) )
                .ToList<AgarElement>();

                if (p.Any())
                {
                    collision.Add( (currentElement.e, p) );
                }
            }
            return collision;
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
