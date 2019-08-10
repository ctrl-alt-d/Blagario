using System;
using Xunit;
using blagario.elements;
using System.Threading.Tasks;

namespace blagario.Unit.Tests
{
    public class PushTests
    {

        private double MassFromRadius(int radius) => Math.Pow( radius,2 )*Math.PI;

        [Fact]
        public async Task OneCloserOther()
        {
            /*                                             

                |<---------15-------->|                                                                                                
                |<----10---->|                                                                                                
                o ---- (---- o ----)--)
                       |<-2->|

            */

            // taken 2 closed cells with 0 speed.
            var universe = new Universe();
            var cell = new Cell(universe);
            var one = new CellPart(universe, cell) {X=500, Y=100, _Mass=MassFromRadius(15)  } ;
            var other = new CellPart(universe, cell) {X=510, Y=100, _Mass=MassFromRadius(2) } ;
            var initialXdistance = Math.Abs( one.X - other.X);
            cell.Add(one);
            cell.Add(other);

            // After Tic
            await one.Tic(0);
            await other.Tic(0);

            // Distance should increase
            var finalXdistance = Math.Abs( one.X - other.X);
            var finalYdistance = Math.Abs( one.Y - other.Y);

            Assert.True( ElementsHelper.Collides(one, other) );
            Assert.True( finalXdistance > initialXdistance );
        }

    }
}
