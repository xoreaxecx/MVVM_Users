using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
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
        private ICommand _showUsersCommand;
        private IReadOnlyCollection<FirebaseObject<UserModel>> _dbUsers;
        private delegate ObservableCollection<string> UserDelegate(IReadOnlyCollection<FirebaseObject<UserModel>> dbUsers);
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

        //public ICommand GetUsersCommand
        //{
        //    get
        //    {
        //        if(_getUsersCommand == null)
        //        {
        //            _getUsersCommand = new RelayCommand(
        //                param => GetUsers());
        //        }
        //        return _getUsersCommand;
        //    }
        //}

        //public ICommand ShowUsersCommand
        //{
        //    get
        //    {
        //        if(_showUsersCommand == null)
        //        {
        //            _showUsersCommand = new RelayCommand(
        //                param => SetCollection());
        //        }
        //        return _showUsersCommand;
        //    }
        //}

        public FirebaseClient FBClient
        {
            get { return _fbClient; }
        }

        #endregion

        #region Helpers

        public UserViewModel()
        {
            UserDelegate callback = SetCollection;
            GetUsers(callback);
            //SetCollection();
            //SetUsers();
            StrCollection.Add("1");
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

        private void ShowUsers()
        {
            foreach (var user in _dbUsers)
            {
                Users.Add(user.Object);
            }
        }

        private async void GetUsers(UserDelegate callback)
        {
            //_dbUsers = await FBClient
            //    .Child("users")
            //    .OrderByKey()
            //    .OnceAsync<UserModel>();//(new TimeSpan(100));

            _dbUsers = await FBClient
                .Child("users")
                .OrderByKey()
                .OnceAsync<UserModel>();//(new TimeSpan(100));

            StrCollection = callback(_dbUsers);
            StrCollection.Add("1");
            //foreach(var user in dbUsers)
            //{
            //    StrCollection.Add(user.Object.Name);
            //}
        }

        public void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private ObservableCollection<string> SetCollection(IReadOnlyCollection<FirebaseObject<UserModel>> dbUsers)
        {
            var result = new ObservableCollection<string>();
            //var itemsLock = new object();
            //BindingOperations.EnableCollectionSynchronization(StrCollection,itemsLock); //!!!
            //StrCollection.Clear();
            foreach (var user in dbUsers)
            {
                //lock (itemsLock)
                    result.Add(user.Object.Name);
            }

            //for (int i = 0; i < 3; i++)
            //{
            //    StrCollection.Add("1");
            //    StrCollection.Add("2");
            //    StrCollection.Add("3");
            //}

            return result;
        }

        #endregion
    }
}
