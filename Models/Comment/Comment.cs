using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    /// <summary>
    /// Comment for project
    /// </summary>
    public class Comment : IComment
    {

        #region Properties

        /// <summary>
        /// CommentID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// Comment date
        /// </summary>
        public DateTime SubmitionDate { get; set; }

        /// <summary>
        /// Project comment
        /// </summary>
        public string Content { get; set; }

        public string Header { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializing Comment object
        /// </summary>
        public Comment()
        {
           
        }

        #endregion

       
    }
}
