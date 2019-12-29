using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Person
{
    public class Contributor : IContributor
    {

        #region Public Properties

        public long ID { get; set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Initials => $"{FirstName.Substring(0, 1)}{LastName.Substring(0, 1)}";

        public string InitialColor { get; private set; }

        #endregion //Public Properties

        #region Constructors

        public Contributor()
        {
            InitialColor = ColourPool.GetColour();
        }

        public Contributor(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            InitialColor = ColourPool.GetColour();
        }

        #endregion //Constructors

        #region Override Methods

        public override string ToString()
        {
            return $"ID: {ID}. {FirstName} {LastName}";
        }

        #endregion //OverrideMethods

    }
}
