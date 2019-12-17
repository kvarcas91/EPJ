using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class Note : INote
    {
        [Key]
        public long ID { get; set; }

        public string Content { get; set; }

        public DateTime SubmitionDate { get; set; }

        public List<IContributor> Contributors => throw new NotImplementedException();
    }
}
