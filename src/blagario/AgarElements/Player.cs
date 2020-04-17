using System;
using System.Threading.Tasks;
using System.Timers;
using BlazorPro.BlazorSize;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace blagario.elements
{
    public class Player: IDisposable
    {
        public Universe Universe;
        public Player(Universe universe)
        {
            Universe = universe;
            Cell = new Cell(this.Universe);
            Universe.World.OnTicReached += OnTicEvent;
        }
        public string FormCss => $@"
            position: absolute;
            top: {XGame2World(CurrentX)}px;
            right: {XGame2World(CurrentY)}px;
            ";

        public string Name {set; get;} 
        public void Play()
        {

            var cellPart = new CellPart(Universe, Cell);
            cellPart.Name = string.IsNullOrEmpty(Name)?"Unnamed cell":Name;
            this.Cell.Add(cellPart);

        }

        private void OnTicEvent(object o, WorldTicEventArgs e) => this.Tic(e.FpsTicNum);

        public Cell Cell {get; private set;}

        public double CurrentX => Cell.X ?? (Universe.World.X/2);
        public double CurrentY => Cell.Y ?? (Universe.World.Y/2);

        /* --- */
        public IJSRuntime JsRuntime {get; private set;}
        public long VisibleAreaX { set; get; } 
        public long VisibleAreaY { set; get; } 

        public float Zoom { set; get; } = 8;

        private float DeltaZoom = 0;

        /* --- */
        public long XGame2Physics( double x )
        {
            var distance_to_cell = ( x - CurrentX ) * this.Zoom;
            var center_point = this.VisibleAreaX / 2;
            var distance_to_center_point = center_point + distance_to_cell;
            return ElementsHelper.TryConvert( distance_to_center_point );
        }
        public long YGame2Physics( double y )
        {
            var distance_to_cell = ( y - CurrentY ) * this.Zoom;
            var center_point = this.VisibleAreaY / 2;
            var distance_to_center_point = center_point + distance_to_cell;
            return ElementsHelper.TryConvert( distance_to_center_point );
        }

        /* --- */
        public long XGame2World( double x )
        {
            return ElementsHelper.TryConvert( x * this.Zoom);
        }
        public long YGame2World( double y )
        {
            return ElementsHelper.TryConvert( y * this.Zoom);
        }

        internal void IncreaseZoom(float v)
        {
            DeltaZoom += v;
        }

        internal void PointTo(double bx, double by)
        {
            Cell.PointTo(bx, by);
        }

        public void Tic(int fpsTicNum)
        {
            this.Cell.Purge();

            if (DeltaZoom<0 && Zoom<0.5)
            {
                DeltaZoom=0;
            }

            if (DeltaZoom>0 && Zoom>9)
            {
                DeltaZoom=0;
            }

            if (DeltaZoom != 0)
            {
                var d = DeltaZoom / 20;
                DeltaZoom -= d;
                Zoom += d;
            }

            if ( !Cell.IsDead && (fpsTicNum+8) % TimedHostedService.fps == 0)
            {                
                Task.Run( async () => await SetFocusToUniverse() );
            }
        }

        /* --- */
        public double XPysics2Game( long x )
        {
            var center_point = this.VisibleAreaX / 2;
            var distance_to_center_point = (x - center_point) / this.Zoom;
            var position = CurrentX + distance_to_center_point;
            return position;
        }

        public double YPysics2Game( long y )
        {
            var center_point = this.VisibleAreaY / 2;
            var distance_to_center_point = (y - center_point) / this.Zoom;
            var position = CurrentY + distance_to_center_point;
            return position;
        }

        public bool OnArea(AgarElement e)
        {
            if (e==null) return false;
            if (e.ElementType == ElementType.Universe ) return true;
            if (e.ElementType == ElementType.World ) return true;
            var nTimesTheDiameter = Math.Max( ( Cell.Diameter ?? 50 ) * 5, 30);
            if (CurrentX - e.X + e.Radius > nTimesTheDiameter ) return false;
            if (e.X - e.Radius - CurrentX > nTimesTheDiameter ) return false;
            if (CurrentY - e.Y + e.Radius > nTimesTheDiameter ) return false;
            if (e.Y - e.Radius - CurrentY > nTimesTheDiameter ) return false;
            return true;
        }

        /* --- */
        private System.Timers.Timer aTimer;

        public async void WindowResized(object _, BrowserWindowSize window)
        {
            this.VisibleAreaX = window.Width;
            this.VisibleAreaY = window.Height;
            Console.WriteLine($" {window} {window.Width} {window.Height} ");
            await Task.CompletedTask;
        }

        public async Task SetFocusToUniverse(IJSRuntime jsRuntime = null)
        {
            JsRuntime = jsRuntime ?? JsRuntime;
            await JsRuntime.InvokeAsync<object>("SetFocusToUniverse");
        }   

        public void Dispose()
        {
            Universe.World.OnTicReached -= OnTicEvent;            
        }

    }
}
