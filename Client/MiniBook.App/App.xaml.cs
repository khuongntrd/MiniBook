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

        }

        private void BuildDependencies()
        {
            if (ServiceLocator.Instance.Built)
                return;

            // Register dependencies

            ServiceLocator.Instance.Build();
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
