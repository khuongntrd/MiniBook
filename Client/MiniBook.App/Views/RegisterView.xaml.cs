
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MiniBook.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterView : ContentPage
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            FirstnameEntry.Focus();
        }
    }
}