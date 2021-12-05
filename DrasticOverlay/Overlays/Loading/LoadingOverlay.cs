using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    public class LoadingOverlay : WindowOverlay
    {
        IWindowOverlayElement _loadingElementOverlay;

        public LoadingOverlay(IWindow window)
            : base(window)
        {
            this._loadingElementOverlay = new LoadingElementOverlay(this);
            AddWindowElement(_loadingElementOverlay);
        }


        class LoadingElementOverlay : IWindowOverlayElement
        {
            readonly IWindowOverlay _overlay;
            RectangleF _backgroundRect = new RectangleF();
            Color _backgroundColor = Color.FromArgb("FF000000");
            Color _loadingSpinnerColor = Colors.White;

            public LoadingElementOverlay(IWindowOverlay overlay, Color? background = null, Color? spinner = null)
            {
                _overlay = overlay;
                if (background != null)
                    this._backgroundColor = background;
                if (spinner != null)
                    _loadingSpinnerColor = spinner;
            }

            public bool Contains(Point point) 
                => this._backgroundRect.Contains(new Point(point.X / _overlay.Density, point.Y / _overlay.Density));

            public void Draw(ICanvas canvas, RectangleF dirtyRect)
            {
                this._backgroundRect = dirtyRect;
                canvas.FillColor = _backgroundColor;
                canvas.StrokeColor = _loadingSpinnerColor;
                canvas.FontColor = _loadingSpinnerColor;
                canvas.FontSize = 45f;

                canvas.FillRectangle(this._backgroundRect);

                canvas.DrawString("Now Loading...", dirtyRect.Width / 2, dirtyRect.Height / 2, HorizontalAlignment.Center);
            }
        }

    }


}
