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

        public override string CssStyle( Eyeglass c ) =>$@"
            top: {c.YGame2Physics(0)}px ;
            left: {c.XGame2Physics(0)}px;
            width: {X.ToString()}px ;
            height: {X.ToString()}px ; 
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

        public override async Task Tic()
        {
            var currentElements = this.Elements.ToList();  // make a copy because TimerService is modifing the array.
            foreach (var e in currentElements)
            {
                await e.Tic();
            }
            CheckIfWoldNedsMoreViruses();            
            await Task.Delay(1);            
            await base.Tic();
        }

        private void CheckIfWoldNedsMoreViruses()
        {
            while( Viruses.Count() < MaxViruses && TotalMass < MaxMass )
            {
                // System.Console.WriteLine($"Creating new viruse. Current mass [{TotalMass}/{MaxMass}]. Current viruses [{Viruses.Count()}/{MaxViruses}]");
                var newViruse = Virus.CreateVirus(this.Universe);
                
            }
        }

    }
}
