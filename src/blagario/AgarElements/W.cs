namespace blagario.elements
{
    public class W: MoveableAgarElement
    {
        private W(Universe universe, long x, long y, long px, long py)
        {
            this.Universe = universe;
            this._Mass = 5;
            this.ElementType = ElementType.W;
            this.Vel = 0.3;
            this.X = x;
            this.Y = y;
            this.PointTo(px, py);
        }

        internal static W CreateW(Universe universe, long x, long y, long vx, long vy)
        {
            return new W(universe,x,y,vx,vy);
        }

    }
}
