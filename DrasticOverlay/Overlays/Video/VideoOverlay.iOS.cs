using AVFoundation;
using AVKit;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;

namespace DrasticOverlay.Overlays
{
    public partial class VideoOverlay
    {
		protected readonly AVPlayerViewController avPlayerViewController = new AVPlayerViewController();
        
		protected NSObject? playedToEndObserver;
        protected IDisposable? statusObserver;
        protected IDisposable? rateObserver;
        protected IDisposable? volumeObserver;
        bool idleTimerDisabled = false;
        AVPlayerItem? playerItem;
		UIView? Element => avPlayerViewController.View;

        public override bool Initialize()
        {
            if (videoOverlayNativeElementsInitialized)
                return true;

            base.Initialize();

			var nativeLayer = Window?.GetNative(true);
			if (nativeLayer is not UIWindow nativeWindow)
				return false;

			if (nativeWindow?.RootViewController?.View == null)
				return false;

			avPlayerViewController.View.BackgroundColor = UIColor.White;
			nativeWindow?.RootViewController.View.AddSubview(avPlayerViewController.View);
			nativeWindow?.RootViewController.View.SendSubviewToBack(nativeWindow);
			return videoOverlayNativeElementsInitialized = true;
        }

        public void SetVideoUrl(string url)
        {
			var asset = AVUrlAsset.Create(NSUrl.FromString(url));
			var playerItem = new AVPlayerItem(asset);
			avPlayerViewController.Player = new AVPlayer(playerItem);
		}

		protected virtual void Play()
		{
			var audioSession = AVAudioSession.SharedInstance();
			var err = audioSession.SetCategory(AVAudioSession.CategoryPlayback);

			//if (err != null)
			//	Log.Warning("MediaElement", "Failed to set AVAudioSession Category {0}", err.Code);

			audioSession.SetMode(AVAudioSession.ModeMoviePlayback, out err);
			//if (err != null)
			//	Log.Warning("MediaElement", "Failed to set AVAudioSession Mode {0}", err.Code);

			err = audioSession.SetActive(true);
			//if (err != null)
			//	Log.Warning("MediaElement", "Failed to set AVAudioSession Active {0}", err.Code);

			if (avPlayerViewController.Player != null)
			{
				avPlayerViewController.Player.Play();
				// UpdateSpeed();
			}

			//if (Element.KeepScreenOn)
			//	SetKeepScreenOn(true);
		}

    }
}
