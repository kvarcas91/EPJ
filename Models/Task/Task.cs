using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class Task : ITask
    {

        #region Properties

        public uint ProjectID { get; private set; }

        public string Description { get; set; }

        public List<IContributor> Contributors { get; } = new List<IContributor>();

        public bool IsCompleted { get; set; }

        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        #endregion

        #region Constructors

        public Task(uint ID)
        {
            ProjectID = ID;
        }

        #endregion


        #region Public Methods



        #endregion

    }
}
