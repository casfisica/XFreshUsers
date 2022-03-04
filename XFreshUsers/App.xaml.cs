using FreshMvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFreshUsers.PageModels;

namespace XFreshUsers
{
    public partial class App : Application
    {
        public static bool isAdmin;
        public App()
        {

            isAdmin = false;

            InitializeComponent();

            var page = FreshPageModelResolver.ResolvePageModel<LogInPageModel>();
            var navContainer = new FreshNavigationContainer(page);
            MainPage = navContainer;

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
