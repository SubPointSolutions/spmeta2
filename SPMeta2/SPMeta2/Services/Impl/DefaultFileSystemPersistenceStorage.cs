using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace SPMeta2.Services.Impl
{
    public class DefaultFileSystemPersistenceStorage : PersistenceStorageServiceBase
    {
        #region constructors

        public DefaultFileSystemPersistenceStorage()
            : this(DefaultFolderPath)
        {

        }

        public DefaultFileSystemPersistenceStorage(string folderPath)
        {
            CurrentFolderPath = folderPath;
        }

        #endregion

        #region static

        static DefaultFileSystemPersistenceStorage()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SPMeta2");
            DefaultFolderPath = path;
        }

        public static string DefaultFolderPath { get; set; }
        public string CurrentFolderPath { get; set; }

        #endregion

        #region methods

        public override byte[] LoadObject(string objectId)
        {
            var fileName = string.Format("{0}.state", objectId);
            var filePath = Path.Combine(CurrentFolderPath, fileName);

            if (File.Exists(filePath))
                return File.ReadAllBytes(filePath);

            return null;
        }

        public override void SaveObject(string objectId, byte[] data)
        {
            Directory.CreateDirectory(CurrentFolderPath);

            var fileName = string.Format("{0}.state", objectId);
            var filePath = Path.Combine(CurrentFolderPath, fileName);

            File.WriteAllBytes(filePath, data);
        }
        #endregion


    }
}
