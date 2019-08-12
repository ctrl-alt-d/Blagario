using System;
using Xunit;
using blagario.elements;
using System.Threading.Tasks;
using System.Linq;

namespace blagario.Unit.Tests
{
    public class SplitTests
    {

        private double MassFromRadius(int radius) => Math.Pow( radius,2 )*Math.PI;

        [Fact]
        public async Task CheckSplit()
        {
            /*                                             

                |<-----1000mass->|                                                                                                
                o----------------) 

            */

            // taken 2 closed cells with 0 speed.
            var universe = new Universe();
            var cell = new Cell(universe);
            var one = new CellPart(universe, cell) {
                X=500, 
                Y=100, 
                _Mass=1000, 
            };
            cell.PointTo(0,0);
            cell.Add(one);

            // After Split for 4 times
            cell.Split();
            await Task.WhenAll( cell.Select(c=>c.Tic(0) ) );
            cell.Split();
            await Task.WhenAll( cell.Select(c=>c.Tic(0) ) );
            cell.Split();
            await Task.WhenAll( cell.Select(c=>c.Tic(0) ) );

            // Distance should increase
            Assert.Equal(8, cell.Count );
            Assert.All<CellPart>( cell, c=> Assert.True(c.X > 400 && c.X < 500) );
        }

    }
}
