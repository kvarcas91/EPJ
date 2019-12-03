using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
   
    public class Contributor : IContributor
    {

        #region Properties

        public long ID { get; set; }

        /// <summary>
        /// Contributor first name
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Contributor last name
        /// </summary>
        public string LastName { get; private set; }

        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Contributors first letter of first name and first letter of last name
        /// </summary>
        public string Initials { 
            get
            {
                return $"{FirstName.Substring(0, 1)}{LastName.Substring(0, 1)}";
            } 
        }

        public string InitialColor { get; private set; }


        #endregion

        #region Constructors

        public Contributor(long ID, string firstName, string lastName)
        {
            this.ID = ID;
            FirstName = firstName;
            LastName = lastName;
            InitialColor = ColourPool.GetColour();
        }

        public Contributor() 
        {
            InitialColor = ColourPool.GetColour();
        }

        #endregion



        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }


    }
}
