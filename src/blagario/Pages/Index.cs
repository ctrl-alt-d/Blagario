using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using blagario.elements;
using Microsoft.AspNetCore.Components;

namespace blagario
{
    public abstract class BaseIndex : ComponentBase, IDisposable
    {

        [Inject] protected World World {get; set; }
        [Inject] protected Cell MyCell {get; set; }

        public void Dispose()
        {
        }

        protected override void OnInit()
        {
            Invoke(
                async () =>
                {
                    while (true)
                    {
                        await Task.Delay(17);
                        StateHasChanged();                    
                    }
                });           
        }
        protected void TrackMouse(UIMouseEventArgs e)
        {
            MyCell.PointTo( e.ClientX, e.ClientY);
        }

    }
}