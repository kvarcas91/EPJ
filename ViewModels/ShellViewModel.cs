using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {

        private string _firstName = "Eddie";
        private string _lastName;
        private Contributor _selectedPerson;
        private BindableCollection<Contributor> _people = new BindableCollection<Contributor>();

        public ShellViewModel()
        {
            People.Add(new Contributor("Eduardas", "Slutas"));
            People.Add(new Contributor("Mindaugas", "Slutas"));
            People.Add(new Contributor("Tomas", "Pluskys"));
        }

        public string FirstName { get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange(() => FirstName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set { 
                _lastName = value;
                NotifyOfPropertyChange(() => LastName);
                NotifyOfPropertyChange(() => FullName);
            }
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public BindableCollection<Contributor> People
        {
            get { return _people; }
            set { _people = value; }
        }

        public Contributor SelectedPerson
        {
            get { return _selectedPerson; }
            set { 
                _selectedPerson = value;
                NotifyOfPropertyChange(() => SelectedPerson);
            }
        }

        public bool CanClearText (string firstName, string lastName)
        {
            return !String.IsNullOrWhiteSpace(firstName) || !String.IsNullOrWhiteSpace(lastName);
        }
        public void ClearText(string firstName, string lastName)
        {

            FirstName = "";
            LastName = "";
        }

        public void LoadPage1()
        {
            //ActivateItem(new FirstChildViewModel());
        }

        public void LoadPage2()
        {
            //ActivateItem(new SecondChildViewModel());
        }
    }
}
