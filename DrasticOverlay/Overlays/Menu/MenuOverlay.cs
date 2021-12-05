using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
	/// <summary>
	/// MenuOverlay shows a basic button and menu system.
	/// By enabling EnableDrawableTouchHandling, you can handle when your overlay elements
	/// are tapped. As a result, you can make basic buttons or other UI.
	/// </summary>
	public class MenuOverlay : WindowOverlay
    {
        IWindowOverlayElement _menuWindowDrawable;

        public MenuOverlay(IWindow window)
            : base(window)
        {
            _menuWindowDrawable = new MenuOverlayElement(this);

            AddWindowElement(_menuWindowDrawable);

            EnableDrawableTouchHandling = true;
            Tapped += OnTapped;
        }

        async void OnTapped(object? sender, WindowOverlayTappedEventArgs e)
        {
            if (!e.WindowOverlayElements.Contains(_menuWindowDrawable))
                return;

			// IWindow does not have page, but Window does.
			// Make note of that when dealing with the inner window for which time of window you have.
            var window = this.Window as Window;
            if (window?.Page == null) return;

            System.Diagnostics.Debug.WriteLine($"Tapped the test overlay button.");

            var result = await window.Page.DisplayActionSheet(
                "Greetings!",
                "Goodbye!",
                null,
                "Do something", "Do something else", "Do something... with feeling.");

            System.Diagnostics.Debug.WriteLine(result);
        }

		class MenuOverlayElement : IWindowOverlayElement
		{
			readonly WindowOverlay _overlay;
			Circle _circle = new Circle(0, 0, 0);
			string logo = "🐒";

			public MenuOverlayElement(WindowOverlay overlay)
			{
				_overlay = overlay;
#if WINDOWS
				logo = "🐱‍🐉";
#elif ANDROID
				logo = "🤖";
#elif IOS
				logo = "A";
#else
				logo = "🐒";
#endif
			}

			public void Draw(ICanvas canvas, RectangleF dirtyRect)
			{
				canvas.FillColor = Color.FromRgba(255, 0, 0, 225);
				canvas.StrokeColor = Color.FromRgba(225, 0, 0, 225);
				canvas.FontColor = Colors.Orange;
				canvas.FontSize = 40f;

				var centerX = dirtyRect.Width - 50;
				var centerY = dirtyRect.Height - 50;
				_circle = new Circle(centerX, centerY, 40);

				canvas.FillCircle(centerX, centerY, 40);
				canvas.DrawString(logo, centerX, centerY + 10, HorizontalAlignment.Center);
			}

			public bool Contains(Point point) =>
				_circle.ContainsPoint(new Point(point.X / _overlay.Density, point.Y / _overlay.Density));

			struct Circle
			{
				public float Radius;
				public PointF Center;

				public Circle(float x, float y, float r)
				{
					Radius = r;
					Center = new PointF(x, y);
				}

				public bool ContainsPoint(Point p) =>
					p.X <= Center.X + Radius &&
					p.X >= Center.X - Radius &&
					p.Y <= Center.Y + Radius &&
					p.Y >= Center.Y - Radius;
			}
		}

	}
}
