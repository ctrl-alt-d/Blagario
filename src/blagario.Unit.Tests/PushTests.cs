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

                |<------15------>|                                                                                                
                |<---10--->|                                                                                                
                      |<---10--->|                                                                                                
                o-----(----)-----o 

            */

            // taken 2 closed cells with 0 speed.
            var universe = new Universe();
            var cell = new Cell(universe);
            var one = new CellPart(universe, cell) {X=500, Y=100, _Mass=MassFromRadius(10)  } ;
            var other = new CellPart(universe, cell) {X=515, Y=100, _Mass=MassFromRadius(10) } ;
            var initialXdistance = Math.Abs( one.X - other.X);
            cell.Add(one);
            cell.Add(other);

            // After Tic
            await one.Tic(0);
            var finalXdistance1 = Math.Abs( one.X - other.X);
            await other.Tic(0);
            var finalXdistance2 = Math.Abs( one.X - other.X);

            // Distance should increase
            

            Assert.True( ElementsHelper.Collides(one, other) );
            Assert.True( finalXdistance1 > initialXdistance );
            Assert.True( finalXdistance2 > finalXdistance1 );
        }

    }
}
