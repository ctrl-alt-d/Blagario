using System;
using System.Threading.Tasks;
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
            Cell = new Cell();
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

            var cellPart = new CellPart(Universe);
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
            return (long) distance_to_center_point;
        }
        public long YGame2Physics( double y )
        {
            var distance_to_cell = ( y - CurrentY ) * this.Zoom;
            var center_point = this.VisibleAreaY / 2;
            var distance_to_center_point = center_point + distance_to_cell;
            return (long) distance_to_center_point;
        }

        /* --- */
        public long XGame2World( double x )
        {
            return (long) ( x * this.Zoom);
        }
        public long YGame2World( double y )
        {
            return (long) ( y * this.Zoom);
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

            if ( (fpsTicNum+8) % TimedHostedService.fps == 0)
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
        private bool resizing = false;
        [JSInvokable]
        public async Task OnBrowserResize()
        {
            try
            {
                await Task.Delay(150);
                await CheckVisibleArea();
            }
            catch
            {

            }
        }

        public async Task CheckVisibleArea(IJSRuntime jsRuntime = null)
        {
            JsRuntime = jsRuntime ?? JsRuntime;
            var visibleArea = await JsRuntime.InvokeAsync<long[]>("GetSize");
            this.VisibleAreaX = visibleArea[0];
            this.VisibleAreaY = visibleArea[1];
            await JsRuntime.InvokeAsync<object>("AreaResized", CreateDotNetObjectRef(this) );
        }            

        public async Task SetFocusToUniverse(IJSRuntime jsRuntime = null)
        {
            JsRuntime = jsRuntime ?? JsRuntime;
            await JsRuntime.InvokeAsync<object>("SetFocusToUniverse");
        }   

        #region Hack to fix https://github.com/aspnet/AspNetCore/issues/11159

        public static object CreateDotNetObjectRefSyncObj = new object();

        protected DotNetObjectRef<T> CreateDotNetObjectRef<T>(T value) where T : class
        {
            lock (CreateDotNetObjectRefSyncObj)
            {
                JSRuntime.SetCurrentJSRuntime(JsRuntime);
                return DotNetObjectRef.Create(value);
            }
        }

        protected void DisposeDotNetObjectRef<T>(DotNetObjectRef<T> value) where T : class
        {
            if (value != null)
            {
                lock (CreateDotNetObjectRefSyncObj)
                {
                    JSRuntime.SetCurrentJSRuntime(JsRuntime);
                    value.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Universe.World.OnTicReached -= OnTicEvent;            
        }

        #endregion

    }
}
