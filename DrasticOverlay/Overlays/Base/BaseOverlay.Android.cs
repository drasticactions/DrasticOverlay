using System.Linq;
using Android.App;
using Android.Views;
using AndroidX.CoordinatorLayout.Widget;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
namespace DrasticOverlay.Overlays
{
    public partial class BaseOverlay
    {
		// Yes, we're cheating here!
		// Normally, we hide these, but I'm gonna open them up
		// So I can poke them from the other classes.
        internal Activity? _nativeActivity;
		internal ViewGroup? _nativeLayer;

		internal Activity? NativeActivity => _nativeActivity;
		internal ViewGroup? NativeLayer => _nativeLayer;

        public virtual bool Initialize()
        {
			return InternalInitialize();
        }

        bool InternalInitialize()
        {
			if (IsNativeViewInitialized)
				return true;

			if (Window == null)
				return false;

			var nativeWindow = Window?.Content?.GetNative(true);
			if (nativeWindow == null)
				return false;

			var handler = Window?.Handler as WindowHandler;
			if (handler?.MauiContext == null)
				return false;
			var rootManager = handler.MauiContext.GetNavigationRootManager();
			if (rootManager == null)
				return false;


			if (handler.NativeView is not Activity activity)
				return false;

			_nativeActivity = activity;
			_nativeLayer = rootManager.RootView as ViewGroup;

			if (_nativeLayer?.Context == null)
				return false;

			if (_nativeActivity?.WindowManager?.DefaultDisplay == null)
				return false;

			var measuredHeight = _nativeLayer.MeasuredHeight;

			if (_nativeActivity.Window != null)
				_nativeActivity.Window.DecorView.LayoutChange += DecorViewLayoutChange;

			if (_nativeActivity?.Resources?.DisplayMetrics != null)
				Density = _nativeActivity.Resources.DisplayMetrics.Density;

		
			return this.IsNativeViewInitialized = true;
		}

		public virtual void DeinitializeNativeDependencies()
		{
			if (_nativeActivity?.Window != null)
				_nativeActivity.Window.DecorView.LayoutChange -= DecorViewLayoutChange;

			IsNativeViewInitialized = false;
		}

		void DecorViewLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
		{
			HandleUIChange();
			Invalidate();
		}
	}
}
