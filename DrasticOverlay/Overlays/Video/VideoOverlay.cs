using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    public partial class VideoOverlay : WindowOverlay
    {
        public VideoOverlay(IWindow window)
            : base(window)
        {
        }

        bool videoOverlayNativeElementsInitialized;

#if !ANDROID && !IOS && !WINDOWS && !MACCATALYST
        public void SetVideoUrl(string url)
        {
        }
#endif
    }
}
