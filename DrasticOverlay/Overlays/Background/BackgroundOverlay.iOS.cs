using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

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

            var nativeLayer = Window?.GetNative(true);
            if (nativeLayer is not UIWindow nativeWindow)
                return false;

            if (nativeWindow?.RootViewController?.View == null)
                return false;

            // "PassthroughView" is a private view for iOS.
            // It contains the graphics view.
            // While we can't reflect on it directly here,
            // You can get the superview, which is the same thing.
            // Just send it to where you want it to go.
            var passthroughView = this.GraphicsView.Superview;
            nativeWindow.RootViewController.View.SendSubviewToBack(passthroughView);

            return backgroundOverlayNativeElementsInitialized = true;
        }
    }
}
