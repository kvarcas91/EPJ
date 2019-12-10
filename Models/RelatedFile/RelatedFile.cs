﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPJ
{
    public class RelatedFile : IRelatedFile
    {

        #region Properties

        /// <summary>
        /// Project ID which holds this file
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// related to the project file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Related to the project file absolute path
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Related to the project file extention
        /// </summary>
        public string FileExtention { get; set; }

        public Icon FileIcon { get; set; }

        /// <summary>
        /// File version
        /// </summary>
        public uint FileVersion { get; set; } = 0;

        #endregion

        #region Constructors

        public RelatedFile() { }

        public RelatedFile(string filePath)
        {
            FilePath = filePath;
            FileName = Path.GetFileNameWithoutExtension(filePath);
            FileExtention = Path.GetExtension(filePath);
           
            if (String.IsNullOrEmpty(FileExtention))
            {
                FileIcon = DefaultIcons.FolderLarge;
            }
            else
            {
                FileIcon = Icon.ExtractAssociatedIcon(filePath);
            }
           
        }

        #endregion

        #region Public Methods

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public void Copy(string newName)
        {
            throw new NotImplementedException();
        }

        public void Replace()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
