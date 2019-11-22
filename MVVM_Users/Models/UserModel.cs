using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_Users
{
    public class UserModel : ObservableObject
    {
        #region Fields

        private string _name;

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

        #endregion

        #region Helpers

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
