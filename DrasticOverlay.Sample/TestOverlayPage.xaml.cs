using DrasticOverlay.Core;
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
    public partial class TestOverlayPage : ContentPage, IHitTestPage
    {
        TestWindow window;
        
        public TestOverlayPage(TestWindow window)
        {
            this.window = window;
            InitializeComponent();
            this.HitTestViews = new List<IView>() { this.TestingView };

        }

        public List<IView> HitTestViews { get; } = new List<IView>();

        private async void OnPageOverlay(object sender, EventArgs e)
        {
            this.window.pageOverlay.RemovePage();
        }
    }
}
