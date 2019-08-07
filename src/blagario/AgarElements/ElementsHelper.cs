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
    }
}