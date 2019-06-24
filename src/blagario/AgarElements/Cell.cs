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
            System.Console.WriteLine("Creating Cell");

        }

        public int Zoom { set; get; } = 10;

        public string MyColor;

        public override async Task Tic() {
            _Mass = _Mass * 0.9999;
            if (_Mass<10) _Mass = 10;
            await base.Tic();
        }

        static string[] availableColors = new string [] {"2ecc71", "3498db", "9b59b6", "f1c40f", "e67e22", "e74c3c" };

        public override string CssStyle(Eyeglass c) => base.CssStyle(c) + $" background-color: #{MyColor}";

        public override void PointTo( long x, long y )
        {
            base.PointTo( x, y );
        }

    }
}
