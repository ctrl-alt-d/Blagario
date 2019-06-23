using System;
using System.Linq;
using System.Threading.Tasks;

namespace blagario.elements
{
    public class Virus : AgarElement
    {
        private Virus(Universe universe, long x, long y)
        {
            this.Universe = universe;
            this._Mass = 100;
            this.ElementType = ElementType.Virus;
            this.X = x;
            this.Y = y;
        }

        internal static Virus CreateVirus(Universe universe)
        {
            var goodPlaceForX = getrandom.Next(0,(int)universe.World.X);
            var goodPlaceForY = getrandom.Next(0,(int)universe.World.Y);

            var v = new Virus(universe, goodPlaceForX, goodPlaceForY);
            universe.World.Elements.Add(v);
            return v;
        }

        public override async Task Tic()
        {
            await base.Tic();
        }

    }
}
