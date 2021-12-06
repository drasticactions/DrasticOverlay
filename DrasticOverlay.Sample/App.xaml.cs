using DrasticOverlay.Overlays;

namespace DrasticOverlay.Sample;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

    // You do not need to create a new window to use the overlays,
    // VisualDiagnostics is created on all Windows by default.
    // You can also add them to the generic Window by using "AddOverlay"
    protected override Window CreateWindow(IActivationState activationState) => new TestWindow() { Page = new MainPage() };
}


internal class TestWindow : Window
{
    internal VideoOverlay videoOverlay;
    internal DragAndDropOverlay dragAndDropOverlay;
    HitDetectionOverlay hitDetectionOverlay;
    BackgroundOverlay backgroundOverlay;
    LoadingOverlay loadingOverlay;
    MenuOverlay menuOverlay;

    public TestWindow()
    {
        this.videoOverlay = new VideoOverlay(this);
        this.hitDetectionOverlay = new HitDetectionOverlay(this) { IsVisible = false };
        this.backgroundOverlay = new BackgroundOverlay(this) { IsVisible = false };
        this.menuOverlay = new MenuOverlay(this);
        this.loadingOverlay = new LoadingOverlay(this) { IsVisible = false };
        this.dragAndDropOverlay = new DragAndDropOverlay(this);
    }

    public void ShowHitDetectionOverlay() => this.hitDetectionOverlay.IsVisible = true;

    public void HideHitDetectionOverlay() => this.hitDetectionOverlay.IsVisible = false;

    public void ShowBackgroundOverlay() => this.backgroundOverlay.IsVisible = true;

    public void HideBackgroundOverlay() => this.backgroundOverlay.IsVisible = false;

    public void ShowLoadingOverlay()
    {
        // For the Loading Overlay, we need to disable touch events so users
        // can't interact with the underlying UI while something is loading.
        this.loadingOverlay.IsVisible = true;
        this.loadingOverlay.DisableUITouchEventPassthrough = true;
    }

    public void HideLoadingOverlay()
    {
        this.loadingOverlay.IsVisible = false;
        this.loadingOverlay.DisableUITouchEventPassthrough = false;
    }

    private void VisualDiagnosticsOverlay_Tapped(object sender, WindowOverlayTappedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(e.Point);

        // if "DisableUITouchEventPassthrough" is enabled, WindowOverlayTappedEventArgs
        // will contain the elements contained within the point that was touched.
        // Otherwise, it will be empty.

        if (e.VisualTreeElements.Any())
        {
            this.Dispatcher.Dispatch(() => {
                VisualDiagnosticsOverlay.RemoveAdorners();
                VisualDiagnosticsOverlay.AddAdorner(e.VisualTreeElements.First(), false);
            });
        }
    }

    protected override void OnCreated()
    {
        // Generally, if you want to add overlays to a Window when it's created,
        // It should go here. This assures that the Window is created, so that
        // the underlying views will be created correctly as well.
        VisualDiagnosticsOverlay.Tapped += VisualDiagnosticsOverlay_Tapped;
        this.AddOverlay(this.backgroundOverlay);
        this.AddOverlay(this.menuOverlay);
        this.AddOverlay(this.loadingOverlay);
        this.AddOverlay(this.hitDetectionOverlay);
        this.AddOverlay(this.dragAndDropOverlay);
        this.AddOverlay(this.videoOverlay);
        base.OnCreated();
    }
}
