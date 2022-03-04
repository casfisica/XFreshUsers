using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using FreshMvvm;
using Xamarin.Forms;

namespace XFreshUsers.PageModels
{
    public class MainPageModel: FreshBasePageModel
    {
        private bool isAdmin;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; RaisePropertyChanged(); }
        }

        public MainPageModel()
        {
            IsAdmin = App.isAdmin;
        }

        public ICommand GoToConfigCommand
        {
            get
            {
                return new Command(async () => {
                    await CoreMethods.PushPageModel<UserListPageModel>();
                });
            }
        }
        

        public ICommand SalirCommand
        {
            get
            {
                return new Command(async () => {

                    await CoreMethods.PopPageModel();
                });
            }
        }
    }
}
