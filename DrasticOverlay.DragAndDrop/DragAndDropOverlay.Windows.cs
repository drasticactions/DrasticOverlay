// <copyright file="DragAndDropOverlay.Windows.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DrasticOverlay
{
	/// <summary>
	/// Drag And Drop Overlay.
	/// </summary>
	public partial class DragAndDropOverlay
	{
		private Microsoft.UI.Xaml.Controls.Panel? panel;

        public override bool Initialize()
        {
            if (this.dragAndDropOverlayNativeElementsInitialized)
            {
                return true;
            }

            base.Initialize();

            var _nativeElement = Window.Content.GetNative(true);
            if (_nativeElement == null)
            {
                return false;
            }

            var handler = Window.Handler as Microsoft.Maui.Handlers.WindowHandler;
            if (handler?.NativeView is not Microsoft.UI.Xaml.Window _window)
            {
                return false;
            }

            this.panel = _window.Content as Microsoft.UI.Xaml.Controls.Panel;
            if (this.panel == null)
            {
                return false;
            }

            this.panel.AllowDrop = true;
            this.panel.DragOver += Panel_DragOver;
            this.panel.Drop += Panel_Drop;
            this.panel.DragLeave += Panel_DragLeave;
            this.panel.DropCompleted += Panel_DropCompleted;
            return dragAndDropOverlayNativeElementsInitialized = true;
        }

        public override bool Deinitialize()
        {
            if (panel != null)
            {
                panel.AllowDrop = false;
                panel.DragOver -= Panel_DragOver;
                panel.Drop -= Panel_Drop;
                panel.DragLeave -= Panel_DragLeave;
                panel.DropCompleted -= Panel_DropCompleted;
            }

            return base.Deinitialize();
        }

        private void Panel_DropCompleted(Microsoft.UI.Xaml.UIElement sender, Microsoft.UI.Xaml.DropCompletedEventArgs args)
        {
            this.IsDragging = false;
        }

        private void Panel_DragLeave(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            this.IsDragging = false;
        }

        private async void Panel_Drop(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            // We're gonna cheat and only take the first item dragged in by the user.
            // In the real world, you would probably want to handle multiple drops and figure
            // Out what to do for your app.
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();
                if (items.Any())
                {
                    var filePaths = new List<string>();
                    foreach(var item in items)
                    {
                        if (item is StorageFile file)
                        {
                            // TODO Add.
                        }
                    }
                    this.Drop?.Invoke(this, new DragAndDropOverlayTappedEventArgs(filePaths));
                }
        }

        private void Panel_DragOver(object sender, Microsoft.UI.Xaml.DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }
    }
}

