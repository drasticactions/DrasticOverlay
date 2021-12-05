using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace DrasticOverlay
{
    internal static class PlatformExtensions
    {
		internal static UIView? GetNative(this IElement view, bool returnWrappedIfPresent)
		{
			if (view.Handler is INativeViewHandler nativeHandler && nativeHandler.NativeView != null)
				return nativeHandler.NativeView;

			return (view.Handler?.NativeView as UIView);

		}
	}
}
