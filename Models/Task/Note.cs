using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class Note : IProjectElement
    {
        public List<IContributor> Contributors { get; } = new List<IContributor>();

        public string Description { get; set; }

        public uint ProjectID { get; private set; }
    }
}
