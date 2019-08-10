using System;
using System.Collections.Generic;
using System.Linq;

namespace blagario.elements
{
    public class Cell: List<CellPart>
    {
        public Cell(Universe universe)
        {
            this.Universe = universe;
        }
        public double? TotalMass => this.Select(c => c._Mass).Sum();
        public double? Diameter => this.Select(c => c.Diameter).Sum();
        public double? X => this.FirstOrDefault()?.X; 
        public double? Y => this.FirstOrDefault()?.Y; 
        public void PointTo(double bx, double by) => this.ForEach(c => c.PointTo(bx, by) );
        public void Purge() => this.RemoveAll(x=>x._Mass == 0);
        public bool IsDead => (TotalMass??0) == 0;
        public bool CurrentlySplitted => this.Count()>=2;
        public IEnumerable<CellPart> SplittableParts => this.Where(e=>e._Mass  >= 35 ).ToList();
        public bool CanSplit => this.Count() <= 12 && SplittableParts.Any();
        public Universe Universe {set; get;}
        public string Name => this.FirstOrDefault()?.Name;

        internal void Split()
        {
            if (IsDead) return;

            foreach( var currentPart in this.SplittableParts)
            {

                currentPart._Mass /= 2;
                var newPart = new CellPart(Universe, currentPart.Cell);
                newPart.Name =this.Name;
                var deltaX = 0.1 * ( newPart.PointingXto > newPart.X ? 1 : -1 );
                var deltaY = 0.1 * ( newPart.PointingYto > newPart.Y ? 1 : -1 );
                newPart.X = this.X.Value+deltaX;
                newPart.Y = this.Y.Value+deltaY;
                newPart._Mass = currentPart._Mass;
                newPart.ChangeVelToSplitVel();
                newPart.PointTo(currentPart.PointingXto, currentPart.PointingYto);
                this.Add(newPart);
            }
        }
    }
}
