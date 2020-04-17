using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blagario.elements;
using BlazorPro.BlazorSize;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
namespace blagario
{
    public abstract class BaseIndex : ComponentBase, IDisposable
    {
        [Inject] protected Universe Universe {get; set; }        
        [Inject] protected Player Player {get; set; }
        [Inject] protected IJSRuntime JsRuntime  {get; set; }
        [Inject] ResizeListener listener {get; set; }
        BrowserWindowSize browser = new BrowserWindowSize();

        public void Dispose() { 
            Universe.World.OnTicReached -= UpdateUi;
        }
        
        protected List<AgarElement> VisibleElements = new List<AgarElement>();

        protected override void OnInitialized()
        {      
            Universe.World.OnTicReached += UpdateUi;
        }
        private void UpdateUi(object sender, EventArgs ea)
        {
            InvokeAsync(
                () =>
                {
                    VisibleElements = Universe
                        .World
                        .Elements
                        .Where( e=> Player.OnArea(e) )
                        .ToList();
                    StateHasChanged();                    
                });
        }
        
        protected void TrackMouse(MouseEventArgs e)
        {
            var bx = Player.XPysics2Game(ElementsHelper.TryConvert(e.ClientX));
            var by = Player.YPysics2Game(ElementsHelper.TryConvert(e.ClientY));
            Player.PointTo( bx, by);
        }

        protected void MouseWheel(WheelEventArgs e)
        {
            Player.IncreaseZoom( - (float)(e.DeltaY/100.0) );
        }

        protected void KeyDown(KeyboardEventArgs e)
        {
            //Issue: KeyDown only is fired when input has focus.
            //System.Console.WriteLine( $"Presset: [{e.Key}]"  );
            switch (e.Key)
            {
                case " ":
                    Player.Cell.Split();
                break;
            }
        }

        protected void OnClick(MouseEventArgs e)
        {
            Player.Cell.Split();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) 
            {
                listener.OnResized += Player.WindowResized;
                await OnAfterFirstRenderAsync();
            }            
        }
        protected async Task OnAfterFirstRenderAsync()
        {
            //this.VisibleAreaX = window.Width;
            //this.VisibleAreaY = window.Height;

            await Player.SetFocusToUniverse(JsRuntime);
        }

    }
}