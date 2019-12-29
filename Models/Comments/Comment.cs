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
        public string Header { get; set; }
        public long ID { get; set; }
        public string Content { get; set; }
        public DateTime SubmitionDate { get; set; }

       
        public DateTime DueDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        #endregion //Public Properties
    }
}
