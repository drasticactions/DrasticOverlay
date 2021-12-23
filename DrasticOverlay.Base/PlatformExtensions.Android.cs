// <copyright file="PlatformExtensions.Android.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System.Numerics;
using System.Threading.Tasks;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using ALayoutDirection = Android.Views.LayoutDirection;
using ATextDirection = Android.Views.TextDirection;
using AView = Android.Views.View;
using GL = Android.Opengl;

namespace DrasticOverlay
{
    public static class PlatformExtensions
    {
        public static AView? GetNative(this IElement view, bool returnWrappedIfPresent)
        {
            if (view.Handler is INativeViewHandler nativeHandler && nativeHandler.NativeView != null)
                return nativeHandler.NativeView;

            return (view.Handler?.NativeView as AView);
        }

        public static NavigationRootManager GetNavigationRootManager(this IMauiContext mauiContext) =>
            mauiContext.Services.GetRequiredService<NavigationRootManager>();

        public static List<T> GetChildView<T>(this Android.Views.ViewGroup view)
        {
            var childCount = view.ChildCount;
            var list = new List<T>();
            for (var i = 0; i < childCount; i++)
            {
                var child = view.GetChildAt(i);
                if (child is T tChild)
                    list.Add(tChild);
            }
            return list;
        }


        public static Microsoft.Maui.Graphics.Rectangle GetBoundingBox(this IView view)
            => view.GetNative(true).GetBoundingBox();

        public static Microsoft.Maui.Graphics.Rectangle GetBoundingBox(this Android.Views.View? nativeView)
        {
            if (nativeView == null)
                return new Rectangle();

            var rect = new Android.Graphics.Rect();
            nativeView.GetGlobalVisibleRect(rect);
            return new Rectangle(rect.ExactCenterX() - (rect.Width() / 2), rect.ExactCenterY() - (rect.Height() / 2), (float)rect.Width(), (float)rect.Height());
        }
    }
}

