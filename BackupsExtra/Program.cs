using System;
using Backups.Entities;
using Backups.Services;
using Backups.Tools;
using BackupsExtra.Entities;
using BackupsExtra.Services.Logging;
using BackupsExtra.Services.RestorePointsLimitsCombination;
using NUnit.Framework;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
        }

        private static void StateUpdateAfterStartCheck()
        {
            var repository1 = new Repository("backup1", @"C:/");
            var backupJob = new BackupJob(repository1.Name, new SingleRestorePointCreationAlgorithm(), repository1);

            var backupManager = new BackupsManager("C:/Users/Andrz/Desktop/info.json");
            backupManager.UpdateState();
            foreach (BackupJobExtra backupJobExtra1 in backupManager.BackupJobsExtra)
            {
                Console.WriteLine(backupJobExtra1.RestorePointExtras.Count);
            }

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

            var repository2 = new Repository("backup2", @"C:/");
            var anotherBack = new BackupJob("ANOTHER", new SplitRestorePointCreationAlgorithm(), repository2);
            BackupJobExtra anotherBackExtra =
                backupManager.AddBackupJobExtra(anotherBack, new AtLeastOneLimitCarriedOut(), new ConsoleLog(false));
            Assert.AreEqual(2, backupJobExtra.GetRestorePoints().Count);
            Assert.AreEqual(3, backupJobExtra.GetStorages().Count);
        }
    }
}