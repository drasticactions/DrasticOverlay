using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    /// <summary>
    /// This is a base implementation of IWindowOverlay.
    /// If you know what you're doing, you can always make your
    /// own IWindowOverlay and do what you want without the graphics layer.
    /// </summary>
    public partial class BaseOverlay : IWindowOverlay
    {
        public BaseOverlay(IWindow window)
        {
            this.Window = window;
        }

        public bool DisableUITouchEventPassthrough { get; set; }
        public bool EnableDrawableTouchHandling { get; set; }

        public bool IsVisible { get; set; }

        public IWindow Window { get; }

        public float Density { get; set; }

        public IReadOnlyCollection<IWindowOverlayElement> WindowElements => new List<IWindowOverlayElement>();

        public bool IsNativeViewInitialized { get; private set; }

        public event EventHandler<WindowOverlayTappedEventArgs> Tapped;

        public bool AddWindowElement(IWindowOverlayElement element)
        {
            return false;
        }

        public bool Deinitialize()
        {
            DeinitializeNativeDependencies();
            return true;
        }

        public void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
        }

        public void HandleUIChange()
        {
        }

        public void Invalidate()
        {
        }

        public bool RemoveWindowElement(IWindowOverlayElement element)
        {
            return false;
        }

        public void RemoveWindowElements()
        {
        }

#if !ANDROID && !IOS && !WINDOWS && !MACCATALYST
        public virtual void DeinitializeNativeDependencies()
        {
        }

        public bool Initialize()
        {
            return false;
        }

#endif
    }
}
