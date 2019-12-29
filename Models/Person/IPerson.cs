using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models.Person
{
    public interface IPerson
    {

        long ID { get; set; }
        string FirstName { get; }
        string LastName { get; }
        string FullName { get;}

    }
}
