namespace blagario.elements
{
    public class W: MoveableAgarElement
    {
        private W(World world, long x, long y, long vx, long vy)
        {
            this.World = world;
            this._Mass = 5;
            this.ElementType = ElementType.W;
            this.Vx = vx;
            this.Vy = vy;
            this.X = x;
            this.Y = y;
        }

        internal static W CreateW(World world, long x, long y, long vx, long vy)
        {
            return new W(world,x,y,vx,vy);
        }

    }
}
