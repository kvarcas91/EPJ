using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public interface IProjectElement
    {

        List<IContributor> Contributors { get; }
        string Description { get; set; }
        uint ProjectID { get; }

    }
}
