using System;
using System.Linq;
using System.Threading.Tasks;

namespace blagario.elements
{
    public class Pellet : AgarElement
    {
        private Pellet(Universe universe, long x, long y)
        {
            this.Universe = universe;
            this._Mass = 1;
            this.ElementType = ElementType.Pellet;
            this.X = x;
            this.Y = y;
            MyColor = availableColors[ getrandom.Next(0, availableColors.Length) ];

        }
        static string[] availableColors = new string [] {"2ecc71", "3498db", "9b59b6", "f1c40f", "e67e22", "e74c3c" };
        public string MyColor;

        public override double Radius => 2;
        public override double Diameter => 4;
        public override string CssStyle(Player c) => $@"
            top: {c.YGame2World(CssY).ToString()}px ;
            left: {c.XGame2World(CssX).ToString()}px ;
            width: 4px ;
            height: 4px ;
            background-color: #{MyColor}";

        internal static Pellet CreatePellet(Universe universe)
        {
            var goodPlaceForX = getrandom.Next(0,(int)universe.World.X);
            var goodPlaceForY = getrandom.Next(0,(int)universe.World.Y);

            var v = new Pellet(universe, goodPlaceForX, goodPlaceForY);
            universe.World.Elements.Add(v);
            return v;
        }

        public override async Task Tic()
        {
            await base.Tic();
        }

    }
}
