using System;
using System.Linq;
using System.Threading.Tasks;

namespace blagario.elements
{
    public class Virus : AgarElement
    {
        private Virus(World world, long x, long y)
        {
            this.World = world;
            this._Mass = 100;
            this.ElementType = ElementType.Virus;
            this.X = x;
            this.Y = y;
        }

        internal static Virus CreateVirus(World world)
        {
            var goodPlaceForX = getrandom.Next(0,(int)world.X);
            var goodPlaceForY = getrandom.Next(0,(int)world.Y);

            var v = new Virus(world, goodPlaceForX, goodPlaceForY);
            world.Elements.Add(v);
            return v;
        }

        public override async Task Tic()
        {
            await base.Tic();
        }

    }
}
