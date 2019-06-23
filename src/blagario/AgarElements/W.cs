namespace blagario.elements
{
    public class W: MoveableAgarElement
    {
        private W(Universe universe, long x, long y, long vx, long vy)
        {
            this.Universe = universe;
            this._Mass = 5;
            this.ElementType = ElementType.W;
            this.Vx = vx;
            this.Vy = vy;
            this.X = x;
            this.Y = y;
        }

        internal static W CreateW(Universe universe, long x, long y, long vx, long vy)
        {
            return new W(universe,x,y,vx,vy);
        }

    }
}
