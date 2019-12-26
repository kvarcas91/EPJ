using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public interface IProjectElement
    {

        long ID { get; set; }

        List<IContributor> Contributors { get; }
        string Content { get; set; }

    }
}
