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
        private UserModel _selectedUser;
        private List<UserModel> _users = new List<UserModel>();
        private List<string> _strUsers = new List<string>();
        private ObservableCollection<string> _stringCollection = new ObservableCollection<string>();
        private ICommand _addUserCommand;
        private ICommand _deleteUserCommand;
        private ICommand _showUsersCommand;
        private ObservableCollection<UserModel> _dbUsers = new ObservableCollection<UserModel>();
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

        public UserModel SelectedtUser
        {
            get { return _selectedUser; }
            set
            {
                if(value != _selectedUser)
                {
                    _selectedUser = value;
                    OnPropertyChanged("SelectedtUser");
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
                    OnPropertyChanged("strUsers");
                }
            }
        }

        public ObservableCollection<UserModel> DbUsers
        {
            get { return _dbUsers; }
            set
            {
                if (value != _dbUsers)
                {
                    _dbUsers = value;
                    OnPropertyChanged("DbUsers");
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
                    OnPropertyChanged("StrCollection");
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
            Task.Run(() => GetUsers());
            //Task t = Task.Factory.StartNew(() => GetUsers());
            //t.Wait();
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
            GetUsers();
        }

        private async void DeleteUser()
        {
            if (SelectedtUser != null && SelectedtUser.Key != null)
            {
                await FBClient
                    .Child("users")
                    .Child(SelectedtUser.Key)
                    .DeleteAsync();
                GetUsers();
            }
        }

        private void ShowUsers()
        {
            //foreach (var user in _dbUsers)
            //{
            //    Users.Add(user.Object);
            //}
        }

        private async void GetUsers()
        {
            var temp = await FBClient
                .Child("users")
                .OrderByKey()
                .OnceAsync<UserModel>();

            await App.Current.Dispatcher.BeginInvoke((Action)delegate () { DbUsers.Clear(); });

            foreach (var e in temp)
            {
                await App.Current.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    DbUsers.Add(new UserModel { Key = e.Key, Name = e.Object.Name });
                });
            }

            //await App.Current.Dispatcher.BeginInvoke((Action)delegate () { StrCollection.Clear(); });

            //foreach (var e in temp)
            //{
            //    await App.Current.Dispatcher.BeginInvoke((Action)delegate ()
            //     {
            //         StrCollection.Add(e.Object.Name);
            //     });
            //}
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
