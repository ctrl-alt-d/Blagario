using System;
using System.Threading.Tasks;

namespace blagario.elements
{
    public static class ElementsHelper
    {
        private static double?[] RadiusFromMassCache = new double?[22001];
        public static double GetRadiusFromMass(long mass)
        {
            var r = RadiusFromMassCache[mass];
            if (r==null)
            {
                r = Math.Sqrt( mass / Math.PI );
                RadiusFromMassCache[mass] = r;
            }
            return r.Value;
        }        

        public static double TwoElementsCenterDistance( AgarElement one, AgarElement other ) => 
            Math.Sqrt(   
                Math.Pow(one.X - other.X, 2) + 
                Math.Pow(one.Y - other.Y, 2) 
            );

        public static bool CanOneElementEatsOtherOneByDistance(AgarElement oneElement, AgarElement otherElement)
        {
            var r = ElementsHelper.TwoElementsCenterDistance(oneElement, otherElement) + otherElement.Radius * 0.4 < oneElement.Radius;
            return r;
        }

        public static bool Collides(AgarElement oneElement, AgarElement otherElement)
        {
            var r = ElementsHelper.TwoElementsCenterDistance(oneElement, otherElement) <= oneElement.Radius +  otherElement.Radius;
            return r;
        }

        public static bool CanOneElementEatsOtherOneByMass(AgarElement oneElement, AgarElement otherElement)
        {
            var r = oneElement._Mass * 0.9 > otherElement._Mass;
            return r;
        }

        public static bool CanOneElementEatsOtherOneByCellGroup(AgarElement oneElement, AgarElement otherElement)
        {
            var bothAreCellParts = oneElement.ElementType != ElementType.CellPart && otherElement.ElementType != ElementType.CellPart;
            if (!bothAreCellParts) return true;

            var oneCellPartElement = oneElement as CellPart;
            var otherCellPartElement = otherElement as CellPart;

            var sameCell = oneCellPartElement.Cell == otherCellPartElement.Cell;
            if (!sameCell) return true;

            var bothEatables = oneCellPartElement.EatableByMySelf && otherCellPartElement.EatableByMySelf;
            return bothEatables;

            
        }

        public static Int64 TryConvert(object n)
        {
            try{
                return Convert.ToInt64(n);
            }
            catch
            {
                return 0;
            }
            
        }
    }
}