using Android.Views;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Android.Views.View;

namespace DrasticOverlay.Overlays
{
    public partial class DragAndDropOverlay
    {
        public override bool Initialize()
        {
            if (dragAndDropOverlayNativeElementsInitialized)
                return true;

            base.Initialize();

            if (this.GraphicsView == null)
                return false;
            this.GraphicsView.GenericMotion += GraphicsView_GenericMotion;
            this.GraphicsView.Drag += GraphicsView_Drag;
            return dragAndDropOverlayNativeElementsInitialized = true;
        }

        private void GraphicsView_GenericMotion(object? sender, GenericMotionEventArgs e)
        {
            Console.WriteLine(e.Event);
        }

        private void GraphicsView_Drag(object? sender, Android.Views.View.DragEventArgs e)
        {
           
        }
    }
}
