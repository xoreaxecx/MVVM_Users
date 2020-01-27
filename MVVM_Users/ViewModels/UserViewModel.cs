using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private List<string> _strUsers = new List<string>();
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

        public List<string> strUsers
        {
            get { return _strUsers; }
            set
            {
                if (value != _strUsers)
                {
                    _strUsers = value;
                    OnPropertyChanged("Users");
                }
            }
        }

        public IReadOnlyCollection<FirebaseObject<UserModel>> dbUsers
        {
            get { return _dbUsers; }
            set
            {
                if (value != _dbUsers)
                {
                    _dbUsers = value;
                    OnPropertyChanged("Users");
                }
            }
        }

        public ObservableCollection<string> StrCollection
        {
            get { return _stringCollection; }
            set
            {
                if (value != _stringCollection)
                {
                    _stringCollection = value;
                    OnPropertyChanged("Users");
                    //foreach(var e in value)
                    //{
                    //    _stringCollection.Add(e);
                    //    OnPropertyChanged("Users");
                    //}
                }
            }
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

        public ICommand ShowUsersCommand
        {
            get
            {
                if (_showUsersCommand == null)
                {
                    _showUsersCommand = new RelayCommand(
                        param => SetCollection());
                }
                return _showUsersCommand;
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
            //Task.Run(() => GetUsers());
            Task t = Task.Factory.StartNew(() => GetUsers());
            t.Wait();
            //StrCollection.Add("1");
            //Users.Add(new UserModel { Name = "Robert Paulson" });
            //strUsers.Add("robert paulson");
        }

        private async void AddUser()
        {
            UserModel newUser = new UserModel();
            newUser.Name = Name;
            Name = string.Empty;
            await FBClient
                .Child("users")
                .PostAsync(newUser,false);
            strUsers.Add("one more");
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

        private async void GetUsers()
        {
            //_dbUsers = await FBClient
            //    .Child("users")
            //    .OrderByKey()
            //    .OnceAsync<UserModel>();//(new TimeSpan(100));

            var temp = await FBClient
                .Child("users")
                .OrderByKey()
                .OnceAsync<UserModel>();//(new TimeSpan(100));

            //foreach (var e in temp)
            //{
            //    Users.Add(e.Object);
            //}

            foreach (var e in temp)
            {
                //strUsers.Add(e.Object.Name);
                //}

                await App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                 {
                     StrCollection.Add(e.Object.Name);
                 });
                //StrCollection = callback(_dbUsers);
                //StrCollection.Add("1");
                //foreach(var user in dbUsers)
                //{
                //    StrCollection.Add(user.Object.Name);
                //}
            }
        }

        public void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void SetCollection()
        {
            //strUsers.Add("btn more");
            //StrCollection.Add("btn more");
        }

        #endregion
    }
}
