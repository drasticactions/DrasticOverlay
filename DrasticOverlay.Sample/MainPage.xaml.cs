using DrasticOverlay.Overlays;

namespace DrasticOverlay.Sample;

public partial class MainPage : ContentPage
{
	bool isNested;

	public MainPage()
	{
		this.isNested = isNested;
		InitializeComponent();
		this.Background = Colors.White;
	}

    protected override void OnAppearing()
    {
		base.OnAppearing();
		((TestWindow)this.GetParentWindow()).dragAndDropOverlay.Drop += DragAndDropOverlay_Drop;
	}

	private void DragAndDropOverlay_Drop(object sender, DragAndDropOverlayTappedEventArgs e)
    {
		if (e == null)
			return;

		this.Dispatcher.Dispatch(() => {
			this.DropFilename.Text = e.Filename;
			this.DropImage.Source = ImageSource.FromStream(() => new MemoryStream(e.File));
		});
    }

	private async void OnPageOverlay(object sender, EventArgs e)
	{
		var window = (TestWindow)this.GetParentWindow();
		window.pageOverlay.SetPage(new TestOverlayPage(window));
	}

	private async void OnVideoOverlay(object sender, EventArgs e)
	{
		this.Background = Colors.Transparent;
		((TestWindow)this.GetParentWindow()).videoOverlay.SetVideoUrl("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4");
	}


	private async void OnDrawAdorners(object sender, EventArgs e)
    {
		// This will disable UI Touch Events passing down to the child elements under them.
		// This will block all UI events beyond this overlay. But it also means
		// We will send more information, such as hit testing for Visual Tree Elements.
		this.GetParentWindow().VisualDiagnosticsOverlay.DisableUITouchEventPassthrough = true;
	}


	private async void OnHitDetectionOverlay(object sender, EventArgs e)
	{
		((TestWindow)this.GetParentWindow()).ShowHitDetectionOverlay();
	}

	private async void OnBackgroundOverlay(object sender, EventArgs e)
    {
		this.Background = Colors.Transparent;
		((TestWindow)this.GetParentWindow()).ShowBackgroundOverlay();
	}

	private async void OnLoadingOverlay(object sender, EventArgs e)
	{

		((TestWindow)this.GetParentWindow()).ShowLoadingOverlay();
		await Task.Delay(4000);
		((TestWindow)this.GetParentWindow()).HideLoadingOverlay();
	}
}

