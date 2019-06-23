using System.Threading.Tasks;

namespace blagario.elements
{
    public class Cell: MoveableAgarElement
    {
        public Cell(Universe universe)
        {
            this.Universe = universe;
            this._Mass = 500; //ToDo: move to 17 some day.
            this.ElementType = ElementType.Cell;
            var goodPlaceForX = getrandom.Next(0,(int)universe.World.X);
            var goodPlaceForY = getrandom.Next(0,(int)universe.World.Y);
            this.X = goodPlaceForX;
            this.Y = goodPlaceForY;            
            universe.World.Elements.Add(this);
            MyColor = availableColors[ getrandom.Next(0, availableColors.Length) ];
        }

        public long VisibleAreaX { set; get; } = 800;  //Todo: get navigator width
        public long VisibleAreaY { set; get; } = 600;  //Todo: get navigator height

        public int Zoom { set; get; } = 10;

        public string MyColor;

        public override async Task Tic() {
            _Mass = _Mass * 0.9999;
            if (_Mass<10) _Mass = 10;
            await base.Tic();
        }

        static string[] availableColors = new string [] {"2ecc71", "3498db", "9b59b6", "f1c40f", "e67e22", "e74c3c" };

        public override string CssStyle(Cell c) => base.CssStyle(c) + $" background-color: #{MyColor}";

        public long translateX(double x) => this.translateX( (long)x );
        public long translateX( long x )
        {
            var b = x - this.X +  ( VisibleAreaX / 2 );
            return b;
        }
        public long translateY(double y) => this.translateX( (long)y );
        public long translateY( long y )
        {
            var b = y - this.Y + ( VisibleAreaY / 2 );
            return b;
        }

        public override void PointTo( long x, long y )
        {
            var bx = x + this.X - ( VisibleAreaX / 2 );
            var by = y + this.Y - ( VisibleAreaY / 2 );
            base.PointTo( x + bx, y + by );
        }

    }
}
