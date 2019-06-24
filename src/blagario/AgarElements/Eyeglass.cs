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
        public long XGame2Physics(double x) => this.XGame2Physics( (long)x );
        public long XGame2Physics( long x )
        {
            var b = x - Cell.X +  ( this.VisibleAreaX / 2 );
            return b;
        }
        public long YGame2Physics(double y) => this.XGame2Physics( (long)y );
        public long YGame2Physics( long y )
        {
            var b = y - Cell.Y + ( this.VisibleAreaY / 2 );
            return b;
        }

        /* --- */
        public long XPysics2Game( int x ) => XPysics2Game( (long)x );
        public long XPysics2Game( long x )
        {
            var bx = x + Cell.X - ( this.VisibleAreaX / 2 );
            return bx;
        }
        public long YPysics2Game( int y ) => YPysics2Game( (long)y );
        public long YPysics2Game( long y )
        {
            var by = y + Cell.Y - ( this.VisibleAreaY / 2 );
            return by;
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
