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
        /// Project ID which holds this comment
        /// </summary>
        public uint ProjectID { get; private set; }

        /// <summary>
        /// Comment date
        /// </summary>
        public DateTime SubmitDate { get; private set; }

        /// <summary>
        /// Project comment
        /// </summary>
        public string ProjectComment { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializing Comment object
        /// </summary>
        /// <param name="ID">project ID</param>
        public Comment(uint ID, string text)
        {
            ProjectID = ID;
            SubmitDate = DateTime.Now;
            ProjectComment = text;
        }

        #endregion

        public override string ToString()
        {
            return $"ID: {ProjectID}; {SubmitDate.ToString("yyyy-MM-dd hh:mm:ss")} {ProjectComment}";
        }
    }
}
