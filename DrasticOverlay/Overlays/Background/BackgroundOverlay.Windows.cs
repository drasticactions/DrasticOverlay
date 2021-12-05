using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    public partial class BackgroundOverlay
    {
        public override bool Initialize()
        {
            if (backgroundOverlayNativeElementsInitialized)
                return true;

            base.Initialize();

            if (this.GraphicsView == null)
                return false;

            // We need to get the window again for the panel.
            // We store it in WindowOverlay, but it's private.
            // Could reflect, but... this is an example. Not gonna bother.
            // Just running the same code again is probably faster.

            var _nativeElement = Window.Content.GetNative(true);
            if (_nativeElement == null)
                return false;

            var handler = Window.Handler as WindowHandler;
            if (handler?.NativeView is not Microsoft.UI.Xaml.Window _window)
                return false;

            var panel =  _window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (panel == null)
                return false;

            panel.Children.Remove(this.GraphicsView);

            // WinUI cares about the ZIndex and the order of the children in the panel.
            // We need to force this child up to the front of the stack, so the 
            // Other views will be rendered... on top.
            this.GraphicsView.SetValue(Microsoft.UI.Xaml.Controls.Canvas.ZIndexProperty, 0);
            panel.Children.Insert(0, this.GraphicsView);

            return backgroundOverlayNativeElementsInitialized = true;
        }
    }
}
