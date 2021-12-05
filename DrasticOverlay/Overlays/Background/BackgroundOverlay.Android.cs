using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Fragment.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    public partial class BackgroundOverlay
    {
        public override bool Initialize()
        {
            if (backgroundOverlayNativeElementsInitialized)
                return true;

            base.Initialize();

            if (this.GraphicsView == null)
                return false;

            // Setting the graphics view to the back.
            var viewGroup = this.GraphicsView.Parent as Android.Views.ViewGroup;
            
            if (viewGroup != null)
            {
                viewGroup.RemoveView(this.GraphicsView);
                var fragmentView = viewGroup.GetChildView<FragmentContainerView>().FirstOrDefault();
                if (fragmentView == null)
                    return false;
                var layerCount = viewGroup.ChildCount;
                viewGroup.AddView(this.GraphicsView, layerCount, new CoordinatorLayout.LayoutParams(CoordinatorLayout.LayoutParams.MatchParent, CoordinatorLayout.LayoutParams.MatchParent));
                fragmentView.BringToFront();
            }


            return backgroundOverlayNativeElementsInitialized = true;
        }
    }
}
