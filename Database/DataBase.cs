using EPJ.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class DataBase
    {

        public static void InsertContributor(IContributor contributor)
        {
            using (var db = new DatabaseModel())
            {
                Database.Contributor contr = new Database.Contributor
                {
                    Id = db.Contributors.Count() + 1,
                    FirstName = contributor.FirstName,
                    LastName = contributor.LastName
                };

                db.Contributors.Add(contr);
                db.SaveChanges();
                db.Dispose();
            }
        }

        public static List<IContributor> GetContributors()
        {
            List<IContributor> contributors = new List<IContributor>(0);
            List<Database.Contributor> dbContributors;

            using (var db = new DatabaseModel())
            {
                dbContributors = (from contributor in db.Contributors select contributor).ToList();
            }

            foreach (var item in dbContributors)
            {
                contributors.Add(new Contributor(item.Id, item.FirstName, item.LastName));
            }


            return contributors;
        }
    }
}

