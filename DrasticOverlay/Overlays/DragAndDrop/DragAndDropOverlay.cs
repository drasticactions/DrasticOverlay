using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    public partial class DragAndDropOverlay : WindowOverlay
    {
        DropElementOverlay dropElement;
        bool dragAndDropOverlayNativeElementsInitialized;

        internal bool IsDragging
        {
            get => dropElement.IsDragging;
            set
            {
                dropElement.IsDragging = value;
                this.Invalidate();
            }
        }

        public DragAndDropOverlay(IWindow window)
            : base(window)
        {
            this.dropElement = new DropElementOverlay();
            this.AddWindowElement(dropElement);
        }

        public event EventHandler<DragAndDropOverlayTappedEventArgs>? Drop;

        class DropElementOverlay : IWindowOverlayElement
        {
            public bool IsDragging { get; set; }
            // We are not going to use Contains for this.
            // We're gonna set if it's invoked externally.
            public bool Contains(Point point) => false;

            public void Draw(ICanvas canvas, RectangleF dirtyRect)
            {
                if (!this.IsDragging)
                    return;

                // We're going to fill the screen with a transparent
                // color to show the drag and drop is happening.
                canvas.FillColor = Color.FromRgba(225, 0, 0, 100);
                canvas.FillRectangle(dirtyRect);
            }
        }
    }

	public class DragAndDropOverlayTappedEventArgs : EventArgs
	{
		public DragAndDropOverlayTappedEventArgs(string filename, byte[] file)
		{
            this.Filename = filename;
            this.File = file;
		}

        public string Filename { get; set; }

        public byte[] File { get; }
	}
}
