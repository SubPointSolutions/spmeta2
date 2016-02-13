using System;
using System.IO;
using System.Reflection;
using SPMeta2.Definitions;
using SPMeta2.Models;

namespace SPMeta2.Syntax.Default.Utils
{
    public static class ModuleFileUtils
    {
        #region from resource helpers

        public static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16 * 1024];

            using (var ms = new MemoryStream())
            {
                int read;

                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);

                return ms.ToArray();
            }
        }


        public static byte[] FromResource(Assembly assembly, string resourceName)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                stream = assembly.GetManifestResourceStream(resourceName);
                reader = new StreamReader(stream);

                return ReadFully(reader.BaseStream);
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
                else if (stream != null)
                    stream.Dispose();
            }
        }

        //public static void LoadModuleFilesFromLocalFolder(ListModelNode hostNode, string folderPath)
        //{
        //    LoadModuleFilesFromLocalFolder((ModelNode) hostNode, folderPath);
        //}

        //public static void LoadModuleFilesFromLocalFolder(FolderModelNode hostNode, string folderPath)
        //{
        //    LoadModuleFilesFromLocalFolder((ModelNode)hostNode, folderPath);
        //}

        public static void LoadModuleFilesFromLocalFolder(IFolderHostModelNode hostNode, string folderPath)
        {
            LoadModuleFilesFromLocalFolder((ModelNode)hostNode, folderPath, null);
        }

        public static void LoadModuleFilesFromLocalFolder(IFolderHostModelNode hostNode, string folderPath, Func<string, bool> shouldIncludeFolderOrFile)
        {
            LoadModuleFilesFromLocalFolder((ModelNode)hostNode, folderPath, shouldIncludeFolderOrFile);
        }

        /// <summary>
        /// Reads the giving directory and populates the node with folders/files structure
        /// </summary>
        /// <param name="hostNode"></param>
        /// <param name="folderPath"></param>
        /// <param name="shouldIncludeFolderOrFile"></param>
        internal static void LoadModuleFilesFromLocalFolder(ModelNode hostNode, string folderPath,
            Func<string, bool> shouldIncludeFolderOrFile)
        {
            var files = Directory.GetFiles(folderPath);
            var folders = Directory.GetDirectories(folderPath);

            foreach (var file in files)
            {
                if (shouldIncludeFolderOrFile != null)
                {
                    if (!shouldIncludeFolderOrFile(file))
                        continue;
                }

                hostNode.AddDefinitionNode(new ModuleFileDefinition
                {
                    Content = File.ReadAllBytes(file),
                    FileName = Path.GetFileName(file),
                    Overwrite = true
                });
            }

            foreach (var subFolder in folders)
            {
                var subFolderPath = subFolder;

                if (shouldIncludeFolderOrFile != null)
                {
                    if (!shouldIncludeFolderOrFile(subFolderPath))
                        continue;
                }

                var folderDef = new FolderDefinition
                {
                    Name = Path.GetFileName(subFolderPath)
                };

                hostNode.AddDefinitionNode(folderDef, folderNode => LoadModuleFilesFromLocalFolder(folderNode, subFolderPath, shouldIncludeFolderOrFile));
            }
        }

        #endregion
    }
}
