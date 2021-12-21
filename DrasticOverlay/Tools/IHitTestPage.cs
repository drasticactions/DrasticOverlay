using System;

namespace DrasticOverlay.Core
{
    /// <summary>
    /// Used for hit testing elements on a given page.
    /// </summary>
    public interface IHitTestPage
    {
        /// <summary>
        /// Gets the elements to be hit tested.
        /// </summary>
        List<IView> HitTestViews { get; }
    }
}

