using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace blagario.elements
{
    public class Player
    {
        public Universe Universe;
        public Player(Universe universe)
        {
            Universe = universe;
            Cell = new Cell(universe);
        }

        public Cell Cell {get; private set;}

        public double CurrentX => Cell!=null?Cell.X:(Universe.X/2);
        public double CurrentY => Cell!=null?Cell.Y:(Universe.Y/2);

        /* --- */
        public IJSRuntime JsRuntime {get; private set;}
        public long VisibleAreaX { set; get; } 
        public long VisibleAreaY { set; get; } 

        public int Zoom { set; get; } = 8;

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
            return (long) (long) ( y * this.Zoom);
        }

        internal void PointTo(double bx, double by)
        {
            Cell.PointTo(bx, by);
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
            var nTimesTheDiameter = Math.Max( Cell.Diameter * 5, 30);
            if (Cell.X - e.X + e.Radius > nTimesTheDiameter ) return false;
            if (e.X - e.Radius - Cell.X > nTimesTheDiameter ) return false;
            if (Cell.Y - e.Y + e.Radius > nTimesTheDiameter ) return false;
            if (e.Y - e.Radius - Cell.Y > nTimesTheDiameter ) return false;
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

        #endregion

    }
}
