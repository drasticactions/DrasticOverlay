using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    /// <summary>
    /// This overlay shows the basics of Hit Detection.
    /// All WindowOverlay's get basic hit testing for free.
    /// When you tap on the screen with an overlay enabled (including the VisualDiagnostics overlay)
    /// You will get a WindowOverlayTappedEventArgs event with the tap and point.
    /// If you enable "EnableDrawableTouchHandling", you will get events when a IWindowElementOverlay
    /// gets tapped and won't send the underlying event under the screen.
    /// If you call "DisableUITouchEventPassthrough", the layer will hold all tap events and not
    /// let anything under be processed. This is useful if you need to block UI interaction for
    /// getting the exact point on screen without pressing buttons or other UI.
    /// </summary>
    public class HitDetectionOverlay : WindowOverlay
    {
        HitDetectionElement hitDetectionElement;
        public HitDetectionOverlay(IWindow window) 
            : base(window)
        {
            this.hitDetectionElement = new HitDetectionElement(this);
            this.EnableDrawableTouchHandling = true;
            this.Tapped += HitDetectionOverlay_Tapped;
            this.AddWindowElement(this.hitDetectionElement);
        }

        private void HitDetectionOverlay_Tapped(object? sender, WindowOverlayTappedEventArgs e)
        {
            var containsBox = this.hitDetectionElement.Contains(e.Point);
            if (containsBox)
            {
                this.hitDetectionElement.CurrentText = "That's me!";
            }
            else
            {
                this.hitDetectionElement.CurrentText = $"x: {e.Point.X} y: {e.Point.Y}";
            }
            this.Invalidate();
        }

        class HitDetectionElement : IWindowOverlayElement
        {
            RectangleF _box = new RectangleF(0, 0, 5000, 25);
            readonly WindowOverlay _overlay;

            public HitDetectionElement(WindowOverlay overlay)
            {
                _overlay = overlay;
            }

            public string CurrentText { get; set; } = "Empty";

            public bool Contains(Point point) =>
                _box.Contains(new Point(point.X / _overlay.Density, point.Y / _overlay.Density));

            public void Draw(ICanvas canvas, RectangleF dirtyRect)
            {
                canvas.FillColor = Colors.Black;
                canvas.StrokeColor = Colors.White;
                canvas.FontColor = Colors.White;
                canvas.FontSize = 15f;
                canvas.FillRectangle(this._box);
                canvas.DrawString(this.CurrentText, _box.X, _box.Y + 15, HorizontalAlignment.Left);
            }
        }
    }
}
