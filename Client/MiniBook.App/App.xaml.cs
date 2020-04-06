// Copyright © 25inc.asia. All rights reserved.

using MiniBook.Services;
using MiniBook.Services.Navigation;
using MiniBook.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace MiniBook
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            BuildDependencies();

            InitNavigation();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            MessagingCenter.Send(this, "OnAppSleep");
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        void BuildDependencies()
        {
            if (ServiceLocator.Instance.Built)
                return;

            // Register dependencies
            ServiceLocator.Instance.RegisterInstance<INavigationService, NavigationService>();
            ServiceLocator.Instance.Register<HttpService>();
            ServiceLocator.Instance.Register<AccountService>();

            ServiceLocator.Instance.RegisterViewModels();

            ServiceLocator.Instance.Build();
        }

        void InitNavigation()
        {
            ServiceLocator.Instance.Resolve<INavigationService>()
                .NavigateToAsync<LoginViewModel>();
        }
    }
}