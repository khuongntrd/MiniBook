using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace MiniBook
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

#if DEBUG
            LiveReload.Init();
#endif

            BuildDependencies();

            InitNavigation();
        }

        private void InitNavigation()
        {
            ServiceLocator.Instance.Resolve<Services.Navigation.INavigationService>()
                .NavigateToAsync<ViewModels.LoginViewModel>();
        }

        private void BuildDependencies()
        {
            if (ServiceLocator.Instance.Built)
                return;

            // Register dependencies
            ServiceLocator.Instance.RegisterInstance<Services.Navigation.INavigationService, Services.Navigation.NavigationService>();
            ServiceLocator.Instance.Register<Services.HttpService>();
            ServiceLocator.Instance.Register<Services.AccountService>();

            ServiceLocator.Instance.RegisterViewModels();

            ServiceLocator.Instance.Build();
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            MessagingCenter.Send<App>(this, "OnAppSleep");
        }

        protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
