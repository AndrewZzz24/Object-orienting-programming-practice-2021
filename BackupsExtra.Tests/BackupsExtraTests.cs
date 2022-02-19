// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Backups.Entities;
using Backups.Services;
using Backups.Tools;
using BackupsExtra.Entities;
using BackupsExtra.Services.Logging;
using BackupsExtra.Services.RestorePointLimits;
using BackupsExtra.Services.RestorePointsDeletingTypes;
using BackupsExtra.Services.RestorePointsLimitsCombination;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupExtraTest
    {
        [Test]
        public void MergeRestorePointsCheckCheck()
        {
            var repository1 = new Repository("backup1", @"C:/");
            var backupJob = new BackupJob(repository1.Name, new SingleRestorePointCreationAlgorithm(), repository1);

            var backupManager = new BackupsManager(null);
            BackupJobExtra backupJobExtra =
                backupManager.AddBackupJobExtra(backupJob, new AllLimitsCarriedOut(), new ConsoleLog(true));

            backupJobExtra.SetStorageType(StorageType.SplitStorage);

            var file1 = new JobObject("C:/Users/Andrz/Desktop/test1.txt");
            var file2 = new JobObject("C:/Users/Andrz/Desktop/test2.txt");

            backupJobExtra.KeepInMemory(true);
            backupJobExtra.AddJobObjectsToBackupJob(file1);
            backupJobExtra.AddJobObjectsToBackupJob(file2);
            backupJobExtra.MakeRestorePoint();
            backupJobExtra.RemoveJobObjectFromBackupJob(file1);
            backupJobExtra.MakeRestorePoint();
            backupJobExtra.CleanRestorePointsList(new AmountRestorePointLimit(1), new MergeRestorePointsCleanType());

            Assert.AreEqual(1, backupJobExtra.GetRestorePoints().Count);
            Assert.AreEqual(2, backupJobExtra.GetStorages().Count);
        }

        [Test]
        public void DeleteRestorePointsCheck()
        {
            var repository1 = new Repository("backup1", @"C:/");
            var backupJob = new BackupJob(repository1.Name, new SingleRestorePointCreationAlgorithm(), repository1);

            var backupManager = new BackupsManager(null);
            BackupJobExtra backupJobExtra =
                backupManager.AddBackupJobExtra(backupJob, new AllLimitsCarriedOut(), new ConsoleLog(true));

            backupJobExtra.SetStorageType(StorageType.SplitStorage);

            var file1 = new JobObject("C:/Users/Andrz/Desktop/test1.txt");
            var file2 = new JobObject("C:/Users/Andrz/Desktop/test2.txt");

            backupJobExtra.KeepInMemory(true);
            backupJobExtra.AddJobObjectsToBackupJob(file1);
            backupJobExtra.AddJobObjectsToBackupJob(file2);
            backupJobExtra.MakeRestorePoint();
            backupJobExtra.RemoveJobObjectFromBackupJob(file1);
            backupJobExtra.MakeRestorePoint();
            backupJobExtra.CleanRestorePointsList(new AmountRestorePointLimit(1), new DeleteRestorePointsCleanType());

            Assert.AreEqual(1, backupJobExtra.GetRestorePoints().Count);
            Assert.AreEqual(1, backupJobExtra.GetStorages().Count);
        }
    }
}