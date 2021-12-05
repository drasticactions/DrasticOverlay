using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    /// <summary>
    /// Background Overlay takes advantage of that WindowOverlay 
    /// has references to the underlying Window Panels.
    /// Normally, we send all overlays to the front of the stack,
    /// but with some light hacking we can move them where we want,
    /// including to the back of the stack.
    /// </summary>
    public partial class BackgroundOverlay : WindowOverlay
    {
        bool backgroundOverlayNativeElementsInitialized;

        IWindowOverlayElement _backgroundElementOverlay;

        public BackgroundOverlay(IWindow window, Color? color = null) : base(window)
        {
            this._backgroundElementOverlay = new BackgroundElementOverlay(this, color);
            this.AddWindowElement(this._backgroundElementOverlay);
        }

        class BackgroundElementOverlay : IWindowOverlayElement
        {
            readonly IWindowOverlay _overlay;
            RectangleF _backgroundRect = new RectangleF();
            Color _backgroundColor = Colors.Green;

            public BackgroundElementOverlay(IWindowOverlay overlay, Color? background = null)
            {
                _overlay = overlay;
                if (background != null)
                    this._backgroundColor = background;
            }

            public bool Contains(Point point)
                => this._backgroundRect.Contains(new Point(point.X / _overlay.Density, point.Y / _overlay.Density));

            public void Draw(ICanvas canvas, RectangleF dirtyRect)
            {
                this._backgroundRect = dirtyRect;
                canvas.FillColor = _backgroundColor;
                canvas.FillRectangle(this._backgroundRect);
            }
        }
    }
}
