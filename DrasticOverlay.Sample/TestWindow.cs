// <copyright file="TestWindow.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;

namespace DrasticOverlay.Sample
{
    /// <summary>
    /// Test Window.
    /// </summary>
    public class TestWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestWindow"/> class.
        /// </summary>
        public TestWindow()
        {
            this.DragAndDropOverlay = new DragAndDropOverlay(this);
        }

        /// <summary>
        /// Gets the drag and drop overlay.
        /// </summary>
        internal DragAndDropOverlay DragAndDropOverlay { get; }

        /// <inheritdoc/>
        protected override void OnCreated()
        {
            this.AddOverlay(this.DragAndDropOverlay);
        }
    }
}