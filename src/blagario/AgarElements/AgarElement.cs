using System;
using System.Threading.Tasks;

namespace blagario.elements
{
    public enum ElementType
    {
        Virus,
        Cell,
        Pellet,
        W
    }
    public class AgarElement
    {
        protected static readonly Random getrandom = new Random();

        public ElementType ElementType {get; protected set; }
        public long X {set; get; }
        public long Y {set; get; }
        protected double _Mass {set; get; }
        public long Mass => (int)_Mass;
        public virtual async Task Tic() 
        { 
            await Task.CompletedTask; 
        }        
        public double Radius => Math.Sqrt( _Mass / Math.PI );
        public double Diameter => Radius * 2;
        public World World {get; protected set;}
        public string CssClass => this.GetType().Name.ToLower();
        public virtual string CssStyle => $@"
            top: {(Y-Radius).ToString()}px ;
            left: {(X-Radius).ToString()}px ;
            width: {Diameter.ToString()}px ;
            height: {Diameter.ToString()}px ;
            ";
    }
}
