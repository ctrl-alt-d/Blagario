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
        CellPart,
        W
    }
    public class AgarElement
    {
        protected static readonly Random getrandom = new Random();
        private Guid key = Guid.NewGuid();
        public string Key => key.ToString();
        public ElementType ElementType {get; protected set; }
        public string Name {get; set;} = "";
        public double X {set; get; }
        public double Y {set; get; }
        public double _Mass {set; get; }
        public double _EatedMass {set; get; } = 0;
        public virtual bool EatableByMySelf {set; get; } = false;
        public long Mass => (int)_Mass;
        public virtual long MaxMass {set; get; } = 20000;
        public virtual async Task Tic(int fpsTicNum) 
        { 
            double eat = 0;
            if ( _EatedMass>0 ) 
            {
                eat = _Mass * 0.01;
                eat = (eat>_EatedMass) ? _EatedMass : eat;
            }

            _EatedMass -= eat;
            _Mass += eat;
            await Task.CompletedTask; 
        }        
        public virtual double Radius => ElementsHelper.GetRadiusFromMass(this.Mass);

        public virtual double Diameter => Radius * 2;
        public long CssX =>ElementsHelper.TryConvert(X-Radius);
        public long CssY =>ElementsHelper.TryConvert(Y-Radius);        
        public Universe Universe {get; protected set;}
        public string CssClass => this.GetType().Name.ToLower();
        public virtual string CssStyle( Player c) => $@"
            top: {(c.YGame2World(CssY)).ToString()}px ;
            left: {(c.XGame2World(CssX)).ToString()}px ;
            width: {(ElementsHelper.TryConvert(Diameter * c.Zoom)).ToString()}px ;
            height: {(ElementsHelper.TryConvert(Diameter * c.Zoom)).ToString()}px ;
            ";
    }
}
