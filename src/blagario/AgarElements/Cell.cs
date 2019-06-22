using System.Threading.Tasks;

namespace blagario.elements
{
    public class Cell: MoveableAgarElement
    {
        public Cell(World world)
        {
            this.World = world;
            this._Mass = 500; //ToDo: move to 17 some day.
            this.ElementType = ElementType.Cell;
            var goodPlaceForX = getrandom.Next(0,(int)world.X);
            var goodPlaceForY = getrandom.Next(0,(int)world.Y);
            this.X = goodPlaceForX;
            this.Y = goodPlaceForY;            
            world.Elements.Add(this);
            MyColor = availableColors[ getrandom.Next(0, availableColors.Length) ];
        }

        public string MyColor;

        public override async Task Tic() {
            _Mass = _Mass * 0.9999;
            if (_Mass<10) _Mass = 10;
            await base.Tic();
        }

        static string[] availableColors = new string [] {"2ecc71", "3498db", "9b59b6", "f1c40f", "e67e22", "e74c3c" };

        public override string CssStyle => base.CssStyle + $" background-color: #{MyColor}";

    }
}
