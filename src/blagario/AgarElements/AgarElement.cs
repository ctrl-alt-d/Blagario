using System;
using System.Threading.Tasks;

namespace blagario.elements
{
    public enum ElementType
    {
        Universe,
        World,
        Virus,
        Pellet,
        Cell,
        W
    }
    public class AgarElement
    {
        protected static readonly Random getrandom = new Random();

        public ElementType ElementType {get; protected set; }
        public double X {set; get; }
        public double Y {set; get; }
        public double _Mass {set; get; }
        public long Mass => (int)_Mass;
        public virtual async Task Tic() 
        { 
            await Task.CompletedTask; 
        }        
        public virtual double Radius => ElementsHelper.GetRadiusFromMass(this.Mass);



        public virtual double Diameter => Radius * 2;
        public long CssX => (long)(X-Radius);
        public long CssY => (long)(Y-Radius);
        public Universe Universe {get; protected set;}
        public string CssClass => this.GetType().Name.ToLower();
        public virtual string CssStyle( Player c) => $@"
            top: {(c.YGame2World(CssY)).ToString()}px ;
            left: {(c.XGame2World(CssX)).ToString()}px ;
            width: {((long)(Diameter * c.Zoom)).ToString()}px ;
            height: {((long)(Diameter * c.Zoom)).ToString()}px ;
            ";
    }
}
