using System.Threading.Tasks;

namespace blagario.elements
{
    public class CellPart: MoveableAgarElement
    {
        public CellPart(Universe universe)
        {
            this.Universe = universe;
            this._Mass = 542; //ToDo: move to 17 some day.
            this.ElementType = ElementType.CellPart;
            var goodPlaceForX = getrandom.Next(0,(int)universe.World.X);
            var goodPlaceForY = getrandom.Next(0,(int)universe.World.Y);
            this.X = goodPlaceForX;
            this.Y = goodPlaceForY;            
            universe.World.Elements.Add(this);
            MyColor = availableColors[ getrandom.Next(0, availableColors.Length) ];
        }

        public override double Vel => 0.2;

        public string MyColor;

        public override async Task Tic(int fpsTicNum) {
            _Mass = _Mass * 0.999995;
            if (_Mass>0 && _Mass<10) _Mass = 10;
            await base.Tic(fpsTicNum);
        }

        static string[] availableColors = new string [] {"2ecc71", "3498db", "9b59b6", "f1c40f", "e67e22", "e74c3c" };

        private string pepaCss => Name=="Pepa"?$@"
            background-image:url('https://i.imgur.com/ZUbWYDl.jpg');
            background-repeat: no-repeat;
            background-size: 100% 100%;":
            "background-color: #{MyColor}";

            
        public override string CssStyle(Player c) => 
            c.Cell.IsDead
            ?"visibility:none"
            :base.CssStyle(c)
            +$@"position: absolute;
            background-color: #{MyColor};";

        public override void PointTo( double x, double y )
        {
            base.PointTo( x, y );
        }

    }
}
