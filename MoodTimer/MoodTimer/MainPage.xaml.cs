using Xamarin.Forms;

namespace MoodTimer
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel();
        }       
    }
}
