using System;
using System.Threading.Tasks;

namespace blagario.elements
{
    public class MoveableAgarElement: AgarElement
    {
        public long Vx {get; set;} = 0;
        public long Vy {get; set;} = 0;

        protected readonly long Vel = 1;

        public void PointTo( long x, long y )
        {

            this.Vx = this.Vel * (x-this.X<0?-1:1);
            this.Vy = this.Vel * (y-this.Y<0?-1:1);

        }

        public override async Task  Tic() {
            this.X += Vx;
            this.Y += Vy;

            this.X = this.X > World.X?World.X:this.X;
            this.Y = this.Y > World.Y?World.Y:this.Y;

            this.X = this.X < 0?0:this.X;
            this.Y = this.Y < 0?0:this.Y;

            await base.Tic();
        }

    }
}
