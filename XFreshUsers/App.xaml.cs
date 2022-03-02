using FreshMvvm;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XFreshUsers.PageModels;

namespace XFreshUsers
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = FreshPageModelResolver.ResolvePageModel<UserListPageModel>();
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
