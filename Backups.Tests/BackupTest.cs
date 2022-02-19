// @author Andrew Zmushko (andrewzmushko@gmail.com)

using Backups.Entities;
using Backups.Services;
using Backups.Tools;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupTest
    {
        [Test]
        public void SaveObjectsInMemory()
        {
            var repository1 = new Repository("backup1", @"C:/");
            var backupJob = new BackupJob(repository1.Name, new SingleRestorePointCreationAlgorithm(), repository1);
            backupJob.StorageType = StorageType.SplitStorage;
            var file1 = new JobObject("C:/Users/Andrz/Desktop/test1.txt");
            var file2 = new JobObject("C:/Users/Andrz/Desktop/test2.txt");
            backupJob.KeepInMemory = true;
            backupJob.AddJobObjectsToBackupJob(file1);
            backupJob.AddJobObjectsToBackupJob(file2);
            backupJob.MakeRestorePoint();
            backupJob.RemoveJobObjectFromBackupJob(file1);
            backupJob.MakeRestorePoint();
            Assert.AreEqual(2, backupJob.GetRestorePoints().Count);
            Assert.AreEqual(3, backupJob.GetStorages().Count);
        }
    }
}