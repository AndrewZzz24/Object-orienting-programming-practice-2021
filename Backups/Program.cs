using Backups.Entities;
using Backups.Services;
using Backups.Tools;
using NUnit.Framework;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
        }

        private static void CreatingBackupsOnPC()
        {
            var repository1 = new Repository("backup1", @"D:/");
            var backupJob = new BackupJob(repository1.Name, new SingleRestorePointCreationAlgorithm(), repository1);
            backupJob.KeepInMemory = false;
            var file1 = new JobObject("C:/Users/Andrz/Desktop/test1.txt");
            var file2 = new JobObject("C:/Users/Andrz/Desktop/test2.txt");
            backupJob.StorageType = StorageType.SingleStorage;
            backupJob.AddJobObjectsToBackupJob(file1).AddJobObjectsToBackupJob(file2);
            backupJob.MakeRestorePoint();
            Assert.AreEqual(1, backupJob.GetRestorePoints().Count);
            Assert.AreEqual(1, backupJob.GetStorages().Count);
        }
    }
}