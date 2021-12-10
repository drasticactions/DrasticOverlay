using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    public partial class PageOverlay
    {
        Microsoft.UI.Xaml.Controls.Panel? panel;
        IMauiContext? mauiContext;


        FrameworkElement? element;

        public override bool Initialize()
        {
            if (pageOverlayNativeElementsInitialized)
                return true;

            base.Initialize();

            var _nativeElement = Window.Content.GetNative(true);
            if (_nativeElement == null)
                return false;

            var handler = Window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.NativeView is not Microsoft.UI.Xaml.Window _window)
                return false;

            if (handler.MauiContext == null)
                return false;

            this.mauiContext = handler.MauiContext;

            this.panel = _window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (panel == null)
                return false;

            return pageOverlayNativeElementsInitialized = true;
        }

        public void SetPage(Page page)
        {
            if (this.panel == null || this.mauiContext == null)
                return;

            if (this.element != null)
                RemovePage();

            this.page = page;
            var pageHandler = page.ToHandler(this.mauiContext);
            element = pageHandler.NativeView;
            if (element != null)
                panel.Children.Add(element);
            pageSet = true;
            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildAdded(this, this.page, 0);
        }

        public void RemovePage()
        {
            if (element == null)
                return;

            panel?.Children.Remove(element);
            pageSet = false;
            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildRemoved(this, this.page, 0);
        }

        public override bool Deinitialize()
        {
            RemovePage();
            return base.Deinitialize();
        }
    }
}
