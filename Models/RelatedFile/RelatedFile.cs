using System;
using System.Collections.Generic;
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
        public uint ProjectID { get; private set; }

        /// <summary>
        /// related to the project file name
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Related to the project file absolute path
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Related to the project file extention
        /// </summary>
        public string FileExtention { get; private set; }

        /// <summary>
        /// File version
        /// </summary>
        public uint FileVersion { get; set; } = 0;

        #endregion

        #region Constructors

        public RelatedFile(uint ID)
        {
            ProjectID = ID;
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
