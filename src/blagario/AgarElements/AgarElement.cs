using System;
using System.Threading.Tasks;

namespace blagario.elements
{
    public enum ElementType
    {
        Universe,
        World,
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
        public long CssX => (long)(X-Radius);
        public long CssY => (long)(Y-Radius);
        public Universe Universe {get; protected set;}
        public string CssClass => this.GetType().Name.ToLower();
        public virtual string CssStyle( Eyeglass c) => $@"
            top: {c.YGame2Physics(CssY).ToString()}px ;
            left: {c.XGame2Physics(CssX).ToString()}px ;
            width: {Diameter.ToString()}px ;
            height: {Diameter.ToString()}px ;
            ";
    }
}
