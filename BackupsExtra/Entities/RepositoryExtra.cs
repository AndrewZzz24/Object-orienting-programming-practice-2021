// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.IO;
using System.IO.Compression;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RepositoryExtra
    {
        public RepositoryExtra(Repository repository)
        {
            Repository = repository;
        }

        public Repository Repository
        {
            get;
        }

        public void DeleteRestorePoint(RestorePointExtra restorePointExtra)
        {
            if (Directory.Exists(restorePointExtra.RestorePoint.Name))
            {
                Directory.Delete(restorePointExtra.RestorePoint.Name);
            }
        }

        public void MakeRestore(RestorePointExtra restorePointExtra, string pathToRestore)
        {
            foreach (Storage storage in restorePointExtra.RestorePoint.GetStorages())
            {
                if (pathToRestore != null)
                {
                    using ZipArchive archive = ZipFile.Open(storage.Name, ZipArchiveMode.Read);
                    foreach (ZipArchiveEntry zippedFile in archive.Entries)
                        zippedFile.ExtractToFile(pathToRestore);
                }
                else
                {
                    using ZipArchive archive = ZipFile.Open(storage.Name, ZipArchiveMode.Read);
                    foreach (JobObject jobObject in storage.GetJobObjects())
                    {
                        foreach (ZipArchiveEntry zippedFile in archive.Entries)
                        {
                            if (zippedFile.FullName.Equals(jobObject.Name))
                                zippedFile.ExtractToFile(jobObject.FileFullName);
                        }
                    }
                }
            }
        }
    }
}