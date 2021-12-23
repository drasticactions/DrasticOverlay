using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using CoreAnimation;
using CoreGraphics;
using ObjCRuntime;
using UIKit;

namespace DrasticOverlay
{
    public static class PlatformExtensions
    {
        public static UIView? GetNative(this IElement view, bool returnWrappedIfPresent)
        {
            if (view.Handler is INativeViewHandler nativeHandler && nativeHandler.NativeView != null)
                return nativeHandler.NativeView;

            return (view.Handler?.NativeView as UIView);

        }

        public static System.Numerics.Matrix4x4 GetViewTransform(this IView view)
        {
            var nativeView = view?.GetNative(true);
            if (nativeView == null)
                return new System.Numerics.Matrix4x4();
            return nativeView.Layer.GetViewTransform();
        }

        public static System.Numerics.Matrix4x4 GetViewTransform(this UIView view)
            => view.Layer.GetViewTransform();

        public static Microsoft.Maui.Graphics.Rectangle GetBoundingBox(this IView view)
            => view.GetNative(true).GetBoundingBox();

        public static Microsoft.Maui.Graphics.Rectangle GetBoundingBox(this UIView? nativeView)
        {
            if (nativeView == null)
                return new Rectangle();
            var nvb = nativeView.GetNativeViewBounds();
            var transform = nativeView.GetViewTransform();
            var radians = transform.ExtractAngleInRadians();
            var rotation = CoreGraphics.CGAffineTransform.MakeRotation((nfloat)radians);
            CGAffineTransform.CGRectApplyAffineTransform(nvb, rotation);
            return new Rectangle(nvb.X, nvb.Y, nvb.Width, nvb.Height);
        }

        public static double ExtractAngleInRadians(this System.Numerics.Matrix4x4 matrix) => Math.Atan2(matrix.M21, matrix.M11);

        public static Rectangle GetNativeViewBounds(this IView view)
        {
            var nativeView = view?.GetNative(true);
            if (nativeView == null)
            {
                return new Rectangle();
            }

            return nativeView.GetNativeViewBounds();
        }

        public static Rectangle GetNativeViewBounds(this UIView nativeView)
        {
            if (nativeView == null)
                return new Rectangle();

            var superview = nativeView;
            while (superview.Superview is not null)
            {
                superview = superview.Superview;
            }

            var convertPoint = nativeView.ConvertRectToView(nativeView.Bounds, superview);

            var X = convertPoint.X;
            var Y = convertPoint.Y;
            var Width = convertPoint.Width;
            var Height = convertPoint.Height;

            return new Rectangle(X, Y, Width, Height);
        }

        public static Matrix4x4 ToViewTransform(this CATransform3D transform) =>
           new Matrix4x4
           {
               M11 = (float)transform.m11,
               M12 = (float)transform.m12,
               M13 = (float)transform.m13,
               M14 = (float)transform.m14,
               M21 = (float)transform.m21,
               M22 = (float)transform.m22,
               M23 = (float)transform.m23,
               M24 = (float)transform.m24,
               M31 = (float)transform.m31,
               M32 = (float)transform.m32,
               M33 = (float)transform.m33,
               M34 = (float)transform.m34,
               Translation = new Vector3((float)transform.m41, (float)transform.m42, (float)transform.m43),
               M44 = (float)transform.m44
           };

        public static Matrix4x4 GetViewTransform(this CALayer layer)
        {
            if (layer == null)
                return new Matrix4x4();

            var superLayer = layer.SuperLayer;
            if (layer.Transform.IsIdentity && (superLayer == null || superLayer.Transform.IsIdentity))
                return new Matrix4x4();

            var superTransform = layer.SuperLayer?.GetChildTransform() ?? CATransform3D.Identity;

            return layer.GetLocalTransform()
                .Concat(superTransform)
                    .ToViewTransform();
        }

        public static CATransform3D Prepend(this CATransform3D a, CATransform3D b) =>
            b.Concat(a);

        public static CATransform3D GetLocalTransform(this CALayer layer)
        {
            return CATransform3D.Identity
                .Translate(
                    layer.Position.X,
                    layer.Position.Y,
                    layer.ZPosition)
                .Prepend(layer.Transform)
                .Translate(
                    -layer.AnchorPoint.X * layer.Bounds.Width,
                    -layer.AnchorPoint.Y * layer.Bounds.Height,
                    -layer.AnchorPointZ);
        }

        public static CATransform3D GetChildTransform(this CALayer layer)
        {
            var childTransform = layer.SublayerTransform;

            if (childTransform.IsIdentity)
                return childTransform;

            return CATransform3D.Identity
                .Translate(
                    layer.AnchorPoint.X * layer.Bounds.Width,
                    layer.AnchorPoint.Y * layer.Bounds.Height,
                    layer.AnchorPointZ)
                .Prepend(childTransform)
                .Translate(
                    -layer.AnchorPoint.X * layer.Bounds.Width,
                    -layer.AnchorPoint.Y * layer.Bounds.Height,
                    -layer.AnchorPointZ);
        }

        public static CATransform3D TransformToAncestor(this CALayer fromLayer, CALayer toLayer)
        {
            var transform = CATransform3D.Identity;

            CALayer? current = fromLayer;
            while (current != toLayer)
            {
                transform = transform.Concat(current.GetLocalTransform());

                current = current.SuperLayer;
                if (current == null)
                    break;

                transform = transform.Concat(current.GetChildTransform());
            }
            return transform;
        }
    }
}

