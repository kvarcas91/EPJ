using EPJ.Models.Components;
using EPJ.Models.Interfaces;
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
        public uint Version { get; set; } = 0;
        public string Path { get; set; }
        public string Name { get; set; }
        public Icon Icon { get; set; }

        public RelatedFolder() { }

        public RelatedFolder(string filePath)
        {
            Path = filePath;
            Name = System.IO.Path.GetFileNameWithoutExtension(filePath);
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

        public bool Delete ()
        {
            Directory.Delete(Path);
            return true;
        }

        public bool Move (string destination)
        {
            Console.WriteLine($"Move from: {Path}");
            Console.WriteLine($"Move to: {destination}");
            var destinationDirectorry = $"{destination}{System.IO.Path.DirectorySeparatorChar}{Name}";
            Directory.Move(Path, destinationDirectorry);
            return true;
        }

        public bool Rename (string newName)
        {
            var parent = Directory.GetParent(Path);
            var destination = ($"{parent}{System.IO.Path.DirectorySeparatorChar}{newName}");
            Directory.Move(Path, destination);
            Path = destination;
            Name = newName;
            return true;
        }

        bool IInteractiveComponent.Copy(string name)
        {
            throw new NotImplementedException();
        }

        bool IInteractiveComponent.Replace(string name)
        {
            throw new NotImplementedException();
        }
    }
}
