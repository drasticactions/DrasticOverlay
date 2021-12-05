using System;
using System.Linq;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.CoreGraphics;
using Microsoft.Maui.Graphics.Native;
using UIKit;

namespace DrasticOverlay.Overlays
{
    public partial class BaseOverlay
    {
        PassthroughView? _passthroughView;

		internal PassthroughView? PassthroughView => _passthroughView;

		public virtual bool Initialize()
        {
			if (IsNativeViewInitialized)
				return true;

			var nativeLayer = Window?.GetNative(true);
			if (nativeLayer is not UIWindow nativeWindow)
				return false;

			if (nativeWindow?.RootViewController?.View == null)
				return false;

			// Create a passthrough view for holding the canvas and other diagnostics tools.
			_passthroughView = new PassthroughView(this, nativeWindow.RootViewController.View.Frame);
			IsNativeViewInitialized = true;
			return IsNativeViewInitialized;
		}

		public virtual void DeinitializeNativeDependencies()
        {
			_passthroughView?.RemoveFromSuperview();
			_passthroughView?.Dispose();
			IsNativeViewInitialized = false;
		}

		void FrameAction(Foundation.NSObservedChange obj)
		{
			HandleUIChange();
			Invalidate();
		}
	}

	internal class PassthroughView : UIView
	{
		/// <summary>
		/// Event Handler for handling on touch events on the Passthrough View.
		/// </summary>
		public event EventHandler<CGPoint>? OnTouch;

		IWindowOverlay overlay;

		/// <summary>
		/// Initializes a new instance of the <see cref="PassthroughView"/> class.
		/// </summary>
		/// <param name="overlay">The Window Overlay.</param>
		/// <param name="frame">Base Frame.</param>
		public PassthroughView(IWindowOverlay windowOverlay, CGRect frame)
			: base(frame)
		{
			overlay = windowOverlay;
		}

		public override bool PointInside(CGPoint point, UIEvent? uievent)
		{
			// If we don't have a UI event, return.
			if (uievent == null)
				return false;

			if (uievent.Type == UIEventType.Hover)
				return false;

			// If we are not pressing down, return.
			if (uievent.Type != UIEventType.Touches)
				return false;

			var disableTouchEvent = false;

			if (overlay.DisableUITouchEventPassthrough)
				disableTouchEvent = true;
			else if (overlay.EnableDrawableTouchHandling)
				disableTouchEvent = overlay.WindowElements.Any(n => n.Contains(new Point(point.X, point.Y)));

			OnTouch?.Invoke(this, point);
			return disableTouchEvent;
		}
	}
}
