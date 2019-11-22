using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_Users
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        App()
        {
            MainWindow app = new MainWindow();
            UserViewModel context = new UserViewModel();
            //context.GetUsers();
            app.DataContext = context;
            //app.TLUsers.ItemsSource = context.Users;
            app.Show();

        }

        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    base.OnStartup(e);

        //    MainWindow app = new MainWindow();
        //    UserViewModel context = new UserViewModel();
        //    app.DataContext = context;
        //    app.Show();
        //}
    }
}
