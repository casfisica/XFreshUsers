using FreshMvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XFreshUsers.Models;
using XFreshUsers.Services;

namespace XFreshUsers.PageModels
{
    public class UserListPageModel : FreshBasePageModel
    {
        private Repository _repository = FreshIOC.Container.Resolve<Repository>();
        private User _selectedUser = null;

        private bool isAdmin;

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { isAdmin = value; RaisePropertyChanged(); }
        }


        /// <summary>
        /// Collection used for binding to the Page's user list view.
        /// </summary>
        public ObservableCollection<User> Users { get; private set; }

        /// <summary>
        /// Used to bind with the list view's SelectedUser property.
        /// Calls the EditUserCommand to start the editing.
        /// </summary>
        public User SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                if (value != null) EditUserCommand.Execute(value);
            }
        }

        public UserListPageModel()
        {
            isAdmin = App.isAdmin;
            Users = new ObservableCollection<User>();
        }

        /// <summary>
        /// Called whenever the page is navigated to.
        /// Here we are ignoring the init data and just loading the users.
        /// </summary>
        public override void Init(object initData)
        {
            isAdmin = App.isAdmin;
            LoadUsers();
            if (Users.Count() < 1)
            {
                CreateSampleData();
            }
        }

        /// <summary>
        /// Called whenever the page is navigated to, but from a pop action.
        /// Here we are just updating the user list with most recent data.
        /// </summary>
        /// <param name="returnedData"></param>
        public override void ReverseInit(object returnedData)
        {
            LoadUsers();
            base.ReverseInit(returnedData);
        }

        /// <summary>
        /// Command associated with the add user action.
        /// Navigates to the UserPageModel with no Init object.
        /// </summary>
        public ICommand AddUserCommand
        {
            get
            {
                return new Command(async () => {
                    await CoreMethods.PushPageModel<UserPageModel>();
                });
            }
        }

        /// <summary>
        /// Command associated with the edit user action.
        /// Navigates to the UserPageModel with the selected user as the Init object.
        /// </summary>
        public ICommand EditUserCommand
        {
            get
            {
                return new Command(async (user) => {
                    await CoreMethods.PushPageModel<UserPageModel>(user);
                });
            }
        }

        /// <summary>
        /// Repopulate the collection with updated users data.
        /// Note: For simplicity, we wait for the async db call to complete,
        /// recommend making better use of the async potential.
        /// </summary>
        private void LoadUsers()
        {
            Users.Clear();
            Task<List<User>> getUserTask = _repository.GetAllUsers();
            getUserTask.Wait();
            foreach (var user in getUserTask.Result)
            {
                if (user.Name == "root") continue;
                Users.Add(user);
            }
        }

        /// <summary>
        /// Uses the SQLite Async capability to insert sample data on multiple threads.
        /// </summary>
        private void CreateSampleData()
        {
            var Admin = new User
            {
                Name = "admin",
                PassWord = "admin",
                IsAdmin = 1
            };

            var root = new User
            {
                Name = "root",
                PassWord = "poioiulkj",
                IsAdmin = 1
            };

            var task1 = _repository.CreateUser(Admin);
            var task2 = _repository.CreateUser(root);

            // Don't proceed until all the async inserts are complete.
            var allTasks = Task.WhenAll(task1, task2);
            allTasks.Wait();

            LoadUsers();
        }
    }
}
