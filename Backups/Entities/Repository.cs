// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.IO;
using System.IO.Compression;
using Backups.Tools;

namespace Backups.Entities
{
    public class Repository
    {
        private int _restorePointNum;
        private string _path;

        public Repository(string name, string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Name = name ?? throw new NullReferenceException();
            Path = path ?? throw new NullReferenceException();
            RestorePointsPath = Path + Name;
        }

        public string Name
        {
            get;
        }

        public string Path
        {
            get => _path;
            internal set
            {
                _path = value;
                RestorePointsPath = _path + Name;
            }
        }

        public string RestorePointsPath
        {
            get;
            private set;
        }

        public void CreateRestorePoint(RestorePoint restorePoint)
        {
            string restorePointPath = RestorePointsPath + "/" + "RestorePoint" + _restorePointNum++;
            restorePoint.Name = restorePointPath;

            Directory.CreateDirectory(restorePointPath);

            int storageNum = 0;

            foreach (Storage storage in restorePoint.GetStorages())
            {
                storageNum++;
                storage.Name = restorePointPath + "/" + storageNum + ".zip";

                CreateArchive(storage, restorePointPath, storageNum);
            }
        }

        private static void CreateArchive(Storage storage, string restorePointPath, int storageNum)
        {
            ZipArchive archive =
                ZipFile.Open(storage.Name, ZipArchiveMode.Create);

            foreach (JobObject jo in storage.GetJobObjects())
            {
                if (jo == null)
                    throw new BackupException("job object cannot be null");

                archive.CreateEntryFromFile(jo.FileFullName, jo.Name);
            }

            archive.Dispose();
        }
    }
}