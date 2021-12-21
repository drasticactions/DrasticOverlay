// <copyright file="PageOverlay.iOS.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using DrasticOverlay.Core;
using ObjCRuntime;
using UIKit;

namespace DrasticOverlay.Overlays
{
    public partial class PageOverlay
    {
        private UIWindow? window;
        private IMauiContext? mauiContext;
        private UIView? element;

        /// <inheritdoc/>
        public override bool Initialize()
        {
            if (this.pageOverlayNativeElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            var handler = this.Window?.Handler;
            if (handler == null)
            {
                return false;
            }

            this.mauiContext = handler.MauiContext;

            var nativeLayer = this.Window?.GetNative(true);
            if (nativeLayer is not UIWindow nativeWindow)
            {
                return false;
            }

            if (nativeWindow?.RootViewController?.View == null)
            {
                return false;
            }

            this.window = nativeWindow;

            return this.pageOverlayNativeElementsInitialized = true;
        }

        public void SetPage(Page page)
        {
            if (this.window?.RootViewController?.View == null || this.mauiContext == null)
            {
                return;
            }

            var view = page.ToHandler(this.mauiContext);
            if (view.NativeView == null)
            {
                return;
            }

            if (this.window?.RootViewController == null)
            {
                return;
            }

            this.element = new HolderView(this, this.window.RootViewController.View.Frame);
            if (this.element != null)
            {
                view.NativeView.Frame = this.element.Frame;
                view.NativeView.AutoresizingMask = UIViewAutoresizing.All;
                this.element.AddSubview(view.NativeView);
                this.element.BringSubviewToFront(view.NativeView);
                this.element.AutoresizingMask = UIViewAutoresizing.All;
                this.window?.RootViewController.View.AddSubview(this.element);
                this.window?.RootViewController.View.BringSubviewToFront(this.element);
            }

            if (page is IHitTestPage hitTestPage)
            {
                foreach (var htElement in hitTestPage.HitTestViews)
                {
                    this.elements.Add(htElement);
                }
            }
        }

        public void RemovePage()
        {
            if (this.element == null)
            {
                return;
            }

            this.elements.Clear();
            this.element.RemoveFromSuperview();
            this.element.Dispose();
            this.pageSet = false;

            Microsoft.Maui.Controls.Xaml.Diagnostics.VisualDiagnostics.OnChildRemoved(this, this.page, 0);
        }

        class HolderView : UIView
        {
            PageOverlay overlay;

            /// <summary>
            /// Initializes a new instance of the <see cref="HolderView"/> class.
            /// </summary>
            /// <param name="overlay">The Window Overlay.</param>
            /// <param name="frame">Base Frame.</param>
            public HolderView(PageOverlay windowOverlay, CGRect frame)
                : base(frame)
            {
                overlay = windowOverlay;
            }

            public override bool PointInside(CGPoint point, UIEvent? uievent)
            {
                foreach (var element in this.overlay.elements)
                {
                    var boundingBox = element.GetBoundingBox();
                    if (boundingBox.Contains(point.X, point.Y))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
