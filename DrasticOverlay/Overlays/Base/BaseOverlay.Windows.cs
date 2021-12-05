using System.Linq;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrasticOverlay.Overlays
{
    public partial class BaseOverlay
    {
        Microsoft.UI.Xaml.Controls.Frame? _frame;
        Panel? _panel;
        FrameworkElement? _nativeElement;

		internal Microsoft.UI.Xaml.Controls.Frame? Frame => _frame;

		internal Microsoft.UI.Xaml.Controls.Panel? Panel => _panel;

		public virtual bool Initialize()
        {
            return InternalInitialize();
        }

        bool InternalInitialize()
        {
			if (IsNativeViewInitialized)
				return true;

			if (Window?.Content == null)
				return false;

			_nativeElement = Window.Content.GetNative(true);
			if (_nativeElement == null)
				return false;
			var handler = Window.Handler as WindowHandler;
			if (handler?.NativeView is not Microsoft.UI.Xaml.Window _window)
				return false;

			_panel = _window.Content as Panel;

			IsNativeViewInitialized = true;
			return IsNativeViewInitialized;
		}

		public virtual void DeinitializeNativeDependencies()
		{
			IsNativeViewInitialized = false;
		}

	}
}
