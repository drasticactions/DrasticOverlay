using Android.Content;
using Android.Media;
using Android.Widget;
using AndroidX.CoordinatorLayout.Widget;
using AndroidX.Fragment.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Overlays
{
    public partial class VideoOverlay
    {
        VideoOverlayView? videoView;

        public override bool Initialize()
        {
            if (videoOverlayNativeElementsInitialized)
                return true;

            base.Initialize();

            var nativeWindow = Window?.Content?.GetNative(true);
            if (nativeWindow?.Context == null)
                return false;

            if (this.GraphicsView == null)
                return false;

            // Get the parent view.
            var viewGroup = this.GraphicsView.Parent as Android.Views.ViewGroup;
            if (viewGroup == null)
                return false;


            this.videoView = new VideoOverlayView(nativeWindow.Context);

            // Force our video view to the back.
            var layerCount = viewGroup.ChildCount;
            var fragmentView = viewGroup.GetChildView<FragmentContainerView>().FirstOrDefault();
            if (fragmentView == null)
                return false;
            viewGroup.AddView(this.videoView, 0, new CoordinatorLayout.LayoutParams(CoordinatorLayout.LayoutParams.MatchParent, CoordinatorLayout.LayoutParams.MatchParent));
            fragmentView.BringToFront();
            return videoOverlayNativeElementsInitialized = true;
        }

        public void SetVideoUrl(string url)
        {
            var uri = Android.Net.Uri.Parse(url);
            this.videoView?.SetVideoURI(uri);
            this.videoView?.Start();
        }

        // This was stolen from the Xamarin Community Toolkit
        // https://github.com/xamarin/XamarinCommunityToolkit
        class VideoOverlayView : VideoView
        {
            public event EventHandler? MetadataRetrieved;

            public VideoOverlayView(Context context)
            : base(context)
            {
                SetBackgroundColor(global::Android.Graphics.Color.Transparent);
            }

            public override async void SetVideoPath(string? path)
            {
                base.SetVideoPath(path);

                if (System.IO.File.Exists(path))
                {
                    var retriever = new MediaMetadataRetriever();

                    await Task.Run(() =>
                    {
                        retriever.SetDataSource(path);
                        ExtractMetadata(retriever);
                        MetadataRetrieved?.Invoke(this, EventArgs.Empty);
                    });
                }
            }

            protected void ExtractMetadata(MediaMetadataRetriever retriever)
            {
                if (int.TryParse(retriever.ExtractMetadata(MetadataKey.VideoWidth), out var videoWidth))
                    VideoWidth = videoWidth;

                if (int.TryParse(retriever.ExtractMetadata(MetadataKey.VideoHeight), out var videoHeight))
                    VideoHeight = videoHeight;

                var durationString = retriever.ExtractMetadata(MetadataKey.Duration);

                if (!string.IsNullOrEmpty(durationString) && long.TryParse(durationString, out var durationMS))
                    DurationTimeSpan = TimeSpan.FromMilliseconds(durationMS);
                else
                    DurationTimeSpan = null;
            }

            public override async void SetVideoURI(global::Android.Net.Uri? uri, IDictionary<string, string>? headers)
            {
                if (uri != null)
                    await SetMetadata(uri, headers);

                base.SetVideoURI(uri, headers);
            }

            protected async Task SetMetadata(global::Android.Net.Uri uri, IDictionary<string, string>? headers)
            {
                var retriever = new MediaMetadataRetriever();

                if (uri.Scheme != null && uri.Scheme.StartsWith(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase))
                {
                    await retriever.SetDataSourceAsync(uri.ToString(), headers ?? new Dictionary<string, string>());
                }
                else
                {
                    await retriever.SetDataSourceAsync(Context, uri);
                }

                ExtractMetadata(retriever);

                MetadataRetrieved?.Invoke(this, EventArgs.Empty);
            }

            public int VideoHeight { get; private set; }

            public int VideoWidth { get; private set; }

            public TimeSpan? DurationTimeSpan { get; private set; }

            public TimeSpan Position => TimeSpan.FromMilliseconds(CurrentPosition);
        }
    }
}
