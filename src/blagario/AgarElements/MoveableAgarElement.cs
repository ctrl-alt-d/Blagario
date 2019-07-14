using System;
using System.Threading.Tasks;

namespace blagario.elements
{
    public class MoveableAgarElement: AgarElement
    {
        protected double Vx {get; private set;} = 0;
        protected double Vy {get; private set;} = 0;

        public virtual double Vel {get; protected set;} = 1;

        public virtual void PointTo( double x, double y )
        {            

            /*              o <---mouse (x,y)
                           /|
                          / |
                         /  |dy
                    Vx  /   |
                    ---o <--- new point
                    | /|    |
                    |/ | Vy | 
            cell--->o----------------
                    |---dx--|

            */

            var dx = x - this.X;
            var dy = y - this.Y;
            var d = Math.Sqrt( dx*dx + dy*dy );
            var sinAlf = dy / d;
            var cosAlf = dx / d;

            this.Vy = this.Vel * sinAlf;
            this.Vx = this.Vel * cosAlf;

        }

        public override async Task  Tic() {
            this.X += Vx;
            this.Y += Vy;

            this.X = this.X > Universe.World.X?Universe.World.X:this.X;
            this.Y = this.Y > Universe.World.Y?Universe.World.Y:this.Y;

            this.X = this.X < 0?0:this.X;
            this.Y = this.Y < 0?0:this.Y;

            await base.Tic();
        }

    }
}
