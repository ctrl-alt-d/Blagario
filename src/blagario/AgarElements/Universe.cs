using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace blagario.elements
{
    public class Universe : AgarElement
    {
        public List<AgarElement> Elements;
        public const long MaxMass = 60 * 1000;
        public const long MaxViruses = 100;  

        public World  World { get; private set;}

        public long MouseX {get; set; }
        public long MouseY {get; set; }

        public override string CssStyle( Player c ) =>$@"
            top: 0px ;
            left: 0px;
            width: 100%;
            height: 100%; 
            "; 

        public Universe()
        {
            this.X = long.MaxValue;
            this.Y = long.MaxValue;
            Elements = new List<AgarElement>();
            World = new World(this);
        }


    }
}
