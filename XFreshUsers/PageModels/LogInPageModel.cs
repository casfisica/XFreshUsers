using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XFreshUsers.Models;
using XFreshUsers.Services;

namespace XFreshUsers.PageModels
{
    public class LogInPageModel : FreshBasePageModel
    {
        private Repository _repository = FreshIOC.Container.Resolve<Repository>();

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; RaisePropertyChanged(); }
        }
        /// <summary>
        /// Se llama cada vez que la pagina aparece
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);
            Clear();
        }

        public void Clear()
        {
            UserName = null;
            PassWord = null;
        }
        private bool CheckUser()
        {
            //Users.Clear();
            Task<List<User>> getUserTask = _repository.GetAllUsers();
            getUserTask.Wait();
            foreach (var usuario in getUserTask.Result)
            {
                if (usuario.Name == UserName && usuario.PassWord == PassWord)
                {
                    App.isAdmin = usuario.IsAdmin == 1; ;
                    return true;
                }
            }
            App.isAdmin = false;
            return false;
        } 

        public ICommand LogIngCommand
        {
            get
            {
                return new Command(async () => {
                    if (CheckUser())
                    {
                        await CoreMethods.PushPageModel<MainPageModel>();
                    }
                    else
                    {
                        await CoreMethods.DisplayAlert("Alerta","Usuario o contraseña incorectas","Cerrar");
                    }
                    
                });
            }
        }

    }
}
