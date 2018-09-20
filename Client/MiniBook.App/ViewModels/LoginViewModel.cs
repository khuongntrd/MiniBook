namespace MiniBook.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {

        }

        private void GoToDashboard()
        {
            NavigationService.NavigateToAsync<DashboardViewModel>();
        }
    }
}
