using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XFreshUsers.Models;
using XFreshUsers.Services;

namespace XFreshUsers.PageModels
{
    public class UserPageModel : FreshBasePageModel
    {
        // Use IoC to get our repository.
        private Repository _repository = FreshIOC.Container.Resolve<Repository>();

        // Backing data model.
        private User _user;

        /// <summary>
        /// Public property exposing the user's name for Page binding.
        /// </summary>
        public string UserName
        {
            get { return _user.Name; }
            set { _user.Name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Public property exposing the user's barcode for Page binding.
        /// </summary>
        public string UserPassWord
        {
            get { return _user.PassWord; }
            set { _user.PassWord = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// Public property exposing the user's quantity for Page binding.
        /// </summary>
        public int UserIsAdmin
        {
            get { return _user.IsAdmin; }
            set 
            {
                if (value==1)
                {
                    _user.IsAdmin = 1; RaisePropertyChanged();
                }
                else 
                { 
                    _user.IsAdmin = 0; RaisePropertyChanged(); 
                }
             }
        }

        /// <summary>
        /// Called whenever the page is navigated to.
        /// Either use a supplied User, or create a new one if not supplied.
        /// FreshMVVM does not provide a RaiseAllPropertyChanged,
        /// so we do this for each bound property, room for improvement.
        /// </summary>
        public override void Init(object initData)
        {
            _user = initData as User;
            if (_user == null) _user = new User();
            base.Init(initData);
            RaisePropertyChanged(nameof(UserName));
            RaisePropertyChanged(nameof(UserPassWord));
        }

        /// <summary>
        /// Command associated with the save action.
        /// Persists the user to the database if the user is valid.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                return new Command(async () => {
                    if (_user.IsValid())
                    {
                        await _repository.CreateUser(_user);
                        await CoreMethods.PopPageModel(_user);
                    }
                });
            }
        }
    }
}
