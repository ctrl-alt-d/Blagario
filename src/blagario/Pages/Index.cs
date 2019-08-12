using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blagario.elements;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace blagario
{

    public abstract class BaseIndex : ComponentBase, IDisposable
    {
        [Inject] protected Universe Universe {get; set; }        
        [Inject] protected Player Player {get; set; }
        [Inject] protected IJSRuntime JsRuntime  {get; set; }

        private bool Rendered = false;
        public void Dispose() { 
            Universe.World.OnTicReached -= UpdateUi;
        }
        
        protected List<AgarElement> VisibleElements = new List<AgarElement>();

        protected override void OnInit()
        {      
            Universe.World.OnTicReached += UpdateUi;
        }
        private void UpdateUi(object sender, EventArgs ea)
        {
            Invoke(
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
        
        protected void TrackMouse(UIMouseEventArgs e)
        {
            var bx = Player.XPysics2Game(ElementsHelper.TryConvert(e.ClientX));
            var by = Player.YPysics2Game(ElementsHelper.TryConvert(e.ClientY));
            Player.PointTo( bx, by);
        }

        protected void MouseWheel(UIWheelEventArgs e)
        {
            Player.IncreaseZoom( - (float)(e.DeltaY/100.0) );
        }

        protected void KeyDown(UIKeyboardEventArgs e)
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

        protected void OnClick(UIMouseEventArgs e)
        {
            Player.Cell.Split();
        }

        protected async override Task OnAfterRenderAsync()
        {
            if (!Rendered) 
            {
                Rendered = true;
                await OnAfterFirstRenderAsync();
            }            
        }
        protected async Task OnAfterFirstRenderAsync()
        {
            await Player.CheckVisibleArea(JsRuntime);    
            await Player.SetFocusToUniverse(JsRuntime);
        }

    }
}