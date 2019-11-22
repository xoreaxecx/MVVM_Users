using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Users
{
    class UserList : ObservableCollection<UserModel>
    {
    }
}


        //<ListView ItemsSource = "{Binding Users}" Grid.RowSpan="4" Grid.Column="1" x:Name="LView" Margin="5,0,0,0">
        //    <ListView.View>
        //        <GridView >
        //            <GridViewColumn Header = "Users"  DisplayMemberBinding="{Binding Path=Name}"  Width="125"></GridViewColumn>
        //        </GridView>
        //    </ListView.View>
        //</ListView>
