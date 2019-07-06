using System;
using System.Threading.Tasks;

namespace blagario.elements
{
    public class MoveableAgarElement: AgarElement
    {
        private double Vx {get; set;} = 0;
        private double Vy {get; set;} = 0;

        public virtual double Vel {get; protected set;} = 1;

        public virtual void PointTo( double x, double y )
        {            
            this.Vx =  this.Vel * (x-this.X<0?-1.0:1.0);
            this.Vy =  this.Vel * (y-this.Y<0?-1.0:1.0);
            System.Console.WriteLine(  $"Nou x y {Vx} {Vy}" );
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
