using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ.Models
{
    public class RelatedFile : IFile
    {

        #region Properties

        /// <summary>
        /// Project ID which holds this file
        /// </summary>
        public ulong ID { get; set; }

        /// <summary>
        /// related to the project file name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Related to the project file absolute path
        /// </summary>
        public string ComponentPath { get; set; }

        /// <summary>
        /// Related to the project file extention
        /// </summary>
        public string Extention { get; set; }

        public Icon Icon { get; set; }

        /// <summary>
        /// File version
        /// </summary>
        public ulong Version { get; set; } = 0;

        #endregion

        #region Constructors

        public RelatedFile() { }

        public RelatedFile(string filePath)
        {
            ComponentPath = filePath;
            Name = Path.GetFileNameWithoutExtension(filePath);
            Extention = Path.GetExtension(filePath);
            Icon = Icon.ExtractAssociatedIcon(filePath);
        }

        #endregion

        #region Public Methods

        public void Copy(string newName)
        {
            throw new NotImplementedException();
        }

        public void Replace(string destination)
        {
            throw new NotImplementedException();
        }

        public void Move (string destination)
        {
            var fullFileDestination = $"{destination}{Path.DirectorySeparatorChar}{Path.GetFileName(ComponentPath)}";
            File.Move(ComponentPath, fullFileDestination);
        }

        public void Rename(string newName)
        {
            var parent = Directory.GetParent(ComponentPath);
            var destination = $"{parent}{Path.DirectorySeparatorChar}{newName}{Extention}";
            File.Move(ComponentPath, destination);
            ComponentPath = destination;
            Name = newName;
        }

        #endregion
    }
}
