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
        public void Dispose() { }

        protected List<AgarElement> VisibleElements = new List<AgarElement>();

        protected override void OnInit()
        {      

            Invoke(
                async () =>
                {
                    while (true)
                    {
                        await Task.Delay(20);
                        VisibleElements = Universe
                            .World
                            .Elements
                            .Where( e=> Player.OnArea(e) )
                            .ToList();
                        StateHasChanged();                    
                    }
                });   
        }
        
        protected void TrackMouse(UIMouseEventArgs e)
        {
            var bx = Player.XPysics2Game(e.ClientX);
            var by = Player.YPysics2Game(e.ClientY);
            Player.PointTo( bx, by);
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
        }
    }
}