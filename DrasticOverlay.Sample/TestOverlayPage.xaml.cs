using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Controls.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrasticOverlay.Sample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestOverlayPage : ContentPage
    {
        TestWindow window;
        
        public TestOverlayPage(TestWindow window)
        {
            this.window = window;
            InitializeComponent();
            UpdateBorderView();
        }

        private void UpdateBorderView()
        {
            var borderShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(2, 2, 2, 2)
            };

            BorderView.StrokeShape = borderShape;
            BorderView.Stroke = new SolidColorBrush(Colors.Black);
        }

        private async void OnPageOverlay(object sender, EventArgs e)
        {
            this.window.pageOverlay.RemovePage();
        }
    }
}
