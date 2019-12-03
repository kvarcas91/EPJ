using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class WindowViewModel : BaseViewModel
    {

        public static WindowViewModel Instance { 
            get
            {
                if (instance != null) return instance;

                lock (syncLock)
                {
                    if (instance == null)
                    {
                        return new WindowViewModel();
                    }
                }
                return instance;
            }
        }

        private WindowViewModel ()
        {
            
        }

        private static volatile WindowViewModel instance = null;

        private static readonly object syncLock = new object();

        public ApplicationPage CurrentPage { 
            get
            {
                return currentPage;
            }
            set
            {
                currentPage = value;
                OnPropertyChanged();
            }
        }

        private ApplicationPage currentPage = ApplicationPage.ProjectList;

    }
}
