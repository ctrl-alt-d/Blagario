using System;
using System.Collections.Generic;
using System.Linq;

namespace blagario.elements
{
    public class Cell: List<CellPart>
    {
        public double? TotalMass => !this.Any()? (double?)null : this.Select(c => c._Mass).Sum();
        public double? Diameter => !this.Any()? (double?)null : this.Select(c => c.Diameter).Sum();
        public double? X => this.OrderByDescending(c=>c._Mass).FirstOrDefault()?.X; //!this.Any()? (double?)null : this.Select(c => c.X * c._Mass).Sum() / ( this.Count() * TotalMass ) ; 
        public double? Y => this.OrderByDescending(c=>c._Mass).FirstOrDefault()?.Y; //!this.Any()? (double?)null : this.Select(c => c.Y * c._Mass).Sum() / ( this.Count() * TotalMass ) ; 
        public void PointTo(double bx, double by) => this.ForEach(c => c.PointTo(bx, by) );
        public void Purge() => this.RemoveAll(x=>x._Mass == 0);
        public bool IsDead => !this.Any() || (TotalMass??0) == 0;
        public bool CurrentlySplitted => this.Count()>=2;
        public Universe Universe => this.FirstOrDefault()?.Universe;
        public string Name => this.FirstOrDefault()?.Name;

        internal void Split()
        {
            if (CurrentlySplitted) return;
            if (IsDead) return;


            var currentPart = this.First();
            currentPart._Mass /= 2;
            var newPart = new CellPart(Universe);
            newPart.Name =this.Name;
            newPart.X = this.X.Value + 100;
            newPart.Y = this.Y.Value + 100;
            newPart._Mass = currentPart._Mass;
            this.Add(newPart);
        }
    }
}
