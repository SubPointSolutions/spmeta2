using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMeta2.Regression.Reports.Html.Zip
{
    public class ZipService
    {
        #region classes

        public class ZipItem
        {
            #region properties

            public string Name { get; set; }
            public byte[] Content { get; set; }

            #endregion
        }

        #endregion

        #region methods

        public MemoryStream Zip(IEnumerable<ZipItem> items)
        {
            return Zip(items, null);
        }

        public MemoryStream Zip(IEnumerable<ZipItem> items, IEnumerable<ZipItem> additionalItems)
        {
            var stream = new MemoryStream();

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                AddItemsToArchive(archive, items);

                if (additionalItems != null)
                    AddItemsToArchive(archive, additionalItems);
            }

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public MemoryStream Zip(string sourceFolderPath)
        {
            return Zip(sourceFolderPath, null);
        }

        public MemoryStream Zip(string sourceFolderPath, IEnumerable<ZipItem> additionalItems)
        {
            var stream = new MemoryStream();

            var entities = new List<ZipItem>();

            ZipFolder(entities, sourceFolderPath, sourceFolderPath);

            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                AddItemsToArchive(archive, entities);

                if (additionalItems != null)
                    AddItemsToArchive(archive, additionalItems);
            }

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        public static void Unzip(string zipFilePath, string extractPath)
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipFilePath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    // if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    // {
                    if (entry.Length > 0)
                    {
                       

                        var filePath = Path.Combine(extractPath, entry.FullName);

                        var dir = Path.GetDirectoryName(filePath);
                        Directory.CreateDirectory(dir);

                        entry.ExtractToFile(filePath);

                    }
                      
                    //}
                }
            }
        }

        private static IEnumerable<ZipItem> GetUniqueEntities(IEnumerable<ZipItem> items)
        {
            var result = new List<ZipItem>();

            foreach (var entity in items)
            {
                if (string.IsNullOrEmpty(entity.Name))
                    continue;

                if (result.FirstOrDefault(e => e.Name.ToLower() == entity.Name.ToLower()) == null)
                    result.Add(entity);
            }

            return result;

        }

        private static void AddItemsToArchive(ZipArchive archive, IEnumerable<ZipItem> items)
        {
            var uniqueEntities = GetUniqueEntities(items);

            foreach (var entity in uniqueEntities)
            {
                if (entity.Content == null)
                {
                    var entityName = entity.Name + "/";
                    archive.CreateEntry(entityName);
                }
                else
                {
                    var entityName = entity.Name;
                    var newZipEntity = archive.CreateEntry(entityName);

                    using (var entryStream = newZipEntity.Open())
                    {
                        using (var entityStreamWriter = new StreamWriter(entryStream))
                        {
                            using (var scontentStream = new MemoryStream(entity.Content))
                            {
                                CopyStream(scontentStream, entityStreamWriter.BaseStream);
                            }
                        }
                    }
                }
            }
        }

        private static void ZipFolder(List<ZipItem> items, string sourceFolderPath, string currentFolderPath)
        {
            var files = Directory.GetFiles(currentFolderPath);
            var folders = Directory.GetDirectories(currentFolderPath);

            foreach (var folderPath in folders)
            {
                var folderName = folderPath.Replace(sourceFolderPath + "\\", string.Empty);

                var newEntity = new ZipItem
                {
                    Name = folderName
                };

                items.Add(newEntity);

                ZipFolder(items, sourceFolderPath, folderPath);
            }

            foreach (var filePath in files)
            {
                var fileName = filePath.Replace(sourceFolderPath + "\\", string.Empty);

                var newEntity = new ZipItem
                {
                    Name = fileName,
                    Content = File.ReadAllBytes(filePath)
                };

                items.Add(newEntity);
            }
        }

        private static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[32768];
            int read;

            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);

        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        #endregion
    }
}
