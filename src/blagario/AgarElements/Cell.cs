using System.Threading.Tasks;

namespace blagario.elements
{
    public class Cell: MoveableAgarElement
    {
        public Cell(Universe universe)
        {
            this.Universe = universe;
            this._Mass = 42; //ToDo: move to 17 some day.
            this.ElementType = ElementType.Cell;
            var goodPlaceForX = getrandom.Next(0,(int)universe.World.X);
            var goodPlaceForY = getrandom.Next(0,(int)universe.World.Y);
            this.X = goodPlaceForX;
            this.Y = goodPlaceForY;            
            universe.World.Elements.Add(this);
            MyColor = availableColors[ getrandom.Next(0, availableColors.Length) ];
        }

        public override double Vel => 0.2;

        public string MyColor;

        public override async Task Tic() {
            _Mass = _Mass * 0.999;
            if (_Mass<10) _Mass = 10;
            await base.Tic();
        }

        static string[] availableColors = new string [] {"2ecc71", "3498db", "9b59b6", "f1c40f", "e67e22", "e74c3c" };

        public override string CssStyle(Player c) => 
            this == c.Cell
            ?$@"
            top: {((long)(-this.Radius*c.Zoom + c.VisibleAreaY/2)).ToString()}px ;
            left: {((long)(-this.Radius*c.Zoom + c.VisibleAreaX/2)).ToString()}px ;
            width: {(Diameter * c.Zoom).ToString()}px ;
            height: {(Diameter * c.Zoom).ToString()}px ;
            background-color: #{MyColor}"
            :base.CssStyle(c)
            +$@"position: absolute;
            background-color: #{MyColor};";

        public override void PointTo( double x, double y )
        {
            base.PointTo( x, y );
        }

    }
}
