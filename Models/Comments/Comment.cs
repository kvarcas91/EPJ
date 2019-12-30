using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Comments
{
    public class Comment : IComment
    {
        #region Public Properties

        [Key]
        public long ID { get; set; }
        public string Header { get; set; }
       
        public string Content { get; set; }
        public DateTime SubmitionDate { get; set; }

       [Computed]
        public DateTime DueDate { get; set; }

        #endregion //Public Properties
    }
}
