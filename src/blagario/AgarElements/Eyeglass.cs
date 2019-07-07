using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace blagario.elements
{
    public class Eyeglass
    {
        public Eyeglass( Cell cell)
        {
            Cell = cell;
        }

        /* --- */
        public IJSRuntime JsRuntime {get; private set;}
        public Cell Cell {get; private set;}
        public long VisibleAreaX { set; get; } 
        public long VisibleAreaY { set; get; } 

        /* --- */
        public long XGame2Physics( double x )
        {
            var distance_to_cell = ( x - Cell.X ) * Cell.Zoom;
            var center_point = this.VisibleAreaX / 2;
            var distance_to_center_point = center_point + distance_to_cell;
            return (long) distance_to_center_point;
        }
        public long YGame2Physics( double y )
        {
            var distance_to_cell = ( y - Cell.Y ) * Cell.Zoom;
            var center_point = this.VisibleAreaY / 2;
            var distance_to_center_point = center_point + distance_to_cell;
            return (long) distance_to_center_point;
        }

        /* --- */
        public long XGame2World( double x )
        {
            return (long) ( x * Cell.Zoom);
        }
        public long YGame2World( double y )
        {
            return (long) (long) ( y * Cell.Zoom);
        }

        /* --- */
        public double XPysics2Game( long x )
        {
            var center_point = this.VisibleAreaX / 2;
            var distance_to_center_point = (x - center_point) / Cell.Zoom;
            var position = Cell.X + distance_to_center_point;
            return position;
        }

        public bool OnArea(AgarElement e)
        {
            if (e==null) return false;
            if (e.ElementType == ElementType.Universe ) return true;
            if (e.ElementType == ElementType.World ) return true;
            var diameter = e.Diameter;
            if (this.XGame2Physics(e.X) < 0 - diameter ) return false;
            if (this.XGame2Physics(e.X) > ( this.VisibleAreaX + diameter) ) return false;
            if (this.YGame2Physics(e.Y) < 0 - diameter ) return false;
            if (this.YGame2Physics(e.Y) > ( this.VisibleAreaY + diameter ) ) return false;
            return true;
        }

        public double YPysics2Game( long y )
        {
            var center_point = this.VisibleAreaY / 2;
            var distance_to_center_point = (y - center_point) / Cell.Zoom;
            var position = Cell.Y + distance_to_center_point;
            return position;
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
