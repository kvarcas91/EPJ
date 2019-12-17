using EPJ.Utilities;
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

        public string Description { get; set; }

        public List<IContributor> Contributors { get; } = new List<IContributor>();

        public bool IsCompleted { get; set; } = false;

        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        #endregion

        #region Constructors

        public Task()
        {

        }

        #endregion


        #region Public Methods



        #endregion

    }
}
