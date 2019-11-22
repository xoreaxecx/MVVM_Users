using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;
using MVVM_Users;


namespace MVVM_Users
{
    public class UserViewModel : ObservableObject
    {
        #region Fields

        private string _name;
        private UserModel _currentUser;
        private List<UserModel> _users = new List<UserModel>();
        private ObservableCollection<string> _stringCollection = new ObservableCollection<string>();
        private ICommand _addUserCommand;
        private ICommand _deleteUserCommand;
        private ICommand _getUsersCommand;
        private readonly FirebaseClient _fbClient = new FirebaseClient("https://mvvmuserlogin.firebaseio.com/");

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set
            {
                if(value != _name)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public UserModel CurrentUser
        {
            get { return _currentUser; }
            set
            {
                if(value != _currentUser)
                {
                    _currentUser = value;
                    OnPropertyChanged("CurrentUser");
                }
            }
        }

        public List<UserModel> Users
        {
            get { return _users; }
            set
            {
                if (value != _users)
                {
                    _users = value;
                    OnPropertyChanged("Users");
                }
            }
        }

        public ObservableCollection<string> StrCollection
        {
            get { return _stringCollection; }
            set{ _stringCollection = value; }
        }

        public ICommand AddUserCommand
        {
            get
            {
                if(_addUserCommand == null)
                {
                    _addUserCommand = new RelayCommand(
                        param => AddUser());
                }
                return _addUserCommand;
            }
        }

        public ICommand DeleteUserCommand
        {
            get
            {
                if(_deleteUserCommand == null)
                {
                    _deleteUserCommand = new RelayCommand(
                        param => DeleteUser());
                }
                return _deleteUserCommand;
            }
        }

        public ICommand GetUsersCommand
        {
            get
            {
                if(_getUsersCommand == null)
                {
                    _getUsersCommand = new RelayCommand(
                        param => GetUsers());
                }
                return _getUsersCommand;
            }
        }

        public FirebaseClient FBClient
        {
            get { return _fbClient; }
        }

        #endregion

        #region Helpers

        public UserViewModel()
        {
            GetUsers();
            SetCollection();
        }

        private async void AddUser()
        {
            UserModel newUser = new UserModel();
            newUser.Name = Name;
            Name = string.Empty;
            await FBClient
                .Child("users")
                .PostAsync(newUser,false);
        }

        private async void DeleteUser()
        {
            await FBClient
                .Child("users")
                .Child(Name)
                .DeleteAsync();
        }

        private async void GetUsers()
        {
            var dbUsers = await FBClient
                .Child("users")
                .OrderByKey()
                .OnceAsync<UserModel>();
            foreach(var user in dbUsers)
            {
                Users.Add(user.Object);
            }

            //foreach(var user in dbUsers)
            //{
            //    StrCollection.Add(user.Object.Name);
            //}
        }

        private void SetCollection()
        {
            for (int i = 0; i < 3; i++)
            {
                StrCollection.Add("1");
                StrCollection.Add("2");
                StrCollection.Add("3");
            }
        }

        #endregion
    }
}
