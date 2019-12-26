using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models
{
    public class RelatedFolder : IFolder
    {
        public ulong ID { get; set; }
        public ulong Version { get; set; } = 0;
        public string ComponentPath { get; set; }
        public string Name { get; set; }
        public Icon Icon { get; set; }

        public RelatedFolder() { }

        public RelatedFolder(string filePath)
        {
            ComponentPath = filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);
            Icon = DefaultIcons.FolderLarge;
        }


        public void Copy(string destination)
        {
            throw new NotImplementedException();
        }

        public void Replace(string destination)
        {
            throw new NotImplementedException();
        }

        public void Move (string destination)
        {
            Console.WriteLine($"Move from: {ComponentPath}");
            Console.WriteLine($"Move to: {destination}");
            var destinationDirectorry = $"{destination}{Path.DirectorySeparatorChar}{Name}";
            Directory.Move(ComponentPath, destinationDirectorry);
        }

        public void Rename (string newName)
        {
            var parent = Directory.GetParent(ComponentPath);
            var destination = ($"{parent}{Path.DirectorySeparatorChar}{newName}");
            Directory.Move(ComponentPath, destination);
            ComponentPath = destination;
            Name = newName;
        }
    }
}
