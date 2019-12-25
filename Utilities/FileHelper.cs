using EPJ.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class FileHelper
    {

        public static List<IComponent> GetFolderContent (string path)
        {
            var output = new List<IComponent>();

            string[] contentDirectories = Directory.GetDirectories(path);
            string[] contentFiles = Directory.GetFiles(path);

            foreach (var item in contentDirectories)
            {
                output.Add(new RelatedFolder(item));
            }
            foreach (var item in contentFiles)
            {
                output.Add(new RelatedFile(item));
            }

            return output;
        }



    }
}
