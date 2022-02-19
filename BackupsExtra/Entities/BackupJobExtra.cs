// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Backups.Entities;
using Backups.Services;
using BackupsExtra.Services.Logging;
using BackupsExtra.Services.RestorePointLimits;
using BackupsExtra.Services.RestorePointsDeletingTypes;
using BackupsExtra.Services.RestorePointsLimitsCombination;
using BackupsExtra.Tools;

namespace BackupsExtra.Entities
{
    public class BackupJobExtra
    {
        public BackupJobExtra(
            BackupJob backupJob,
            ILimitsCombination limitsCombination,
            IBackupsLog log,
            BackupsManager backupsManager)
        {
            BackupJob = backupJob ?? throw new BackupsExtraException("backup job cannot be null");
            LimitsCombination = limitsCombination;
            RestorePointExtras = new List<RestorePointExtra>();
            RepositoryExtra = new RepositoryExtra(backupJob.Repository);
            Log = log;
            BackupsManager = backupsManager;
        }

        public IBackupsLog Log
        {
            get;
        }

        public RepositoryExtra RepositoryExtra
        {
            get;
        }

        public ILimitsCombination LimitsCombination
        {
            get;
        }

        public List<RestorePointExtra> RestorePointExtras
        {
            get;
            internal set;
        }

        public BackupJob BackupJob
        {
            get;
        }

        private BackupsManager BackupsManager
        {
            get;
        }

        public void SetStorageType(StorageType storageType)
        {
            BackupJob.StorageType = storageType;
            MakeLogs("Storage Type changed");
            BackupsManager.SaveState();
        }

        public void KeepInMemory(bool value)
        {
            BackupJob.KeepInMemory = value;
            MakeLogs("Keep in memory boolean value changed");
            BackupsManager.SaveState();
        }

        public RestorePointExtra MakeRestorePoint()
        {
            RestorePoint restorePoint = BackupJob.MakeRestorePoint();
            var result = new RestorePointExtra(restorePoint, DateTime.Now);

            RestorePointExtras.Add(result);

            MakeLogs("Restore point has been made " + DateTime.Now);
            BackupsManager.SaveState();
            return result;
        }

        public BackupJobExtra AddJobObjectsToBackupJob(params JobObject[] jobObject)
        {
            BackupJob.AddJobObjectsToBackupJob(jobObject);
            MakeLogs("Job objects added");
            BackupsManager.SaveState();
            return this;
        }

        public BackupJobExtra RemoveJobObjectFromBackupJob(JobObject jobObject)
        {
            BackupJob.RemoveJobObjectFromBackupJob(jobObject);
            MakeLogs("Job objects removed");
            BackupsManager.SaveState();
            return this;
        }

        public List<RestorePointExtra> GetRestorePoints()
        {
            return RestorePointExtras;
        }

        public List<Storage> GetStorages()
        {
            BackupsManager.SaveState();
            return BackupJob.GetStorages();
        }

        public void CleanRestorePointsList(
            IRestorePointLimit restorePointLimit,
            IRestorePointCleanType cleanType)
        {
            RestorePointExtras = cleanType.Evaluate(restorePointLimit, RestorePointExtras, RepositoryExtra);
            var restorePointsToBackupJob = new List<RestorePoint>();
            foreach (RestorePointExtra restorePointExtra in RestorePointExtras)
            {
                restorePointsToBackupJob.Add(restorePointExtra.RestorePoint);
            }

            BackupJob.RestorePoints = restorePointsToBackupJob;
            MakeLogs("Restore points cleaning operation done");
            BackupsManager.SaveState();
        }

        public void MakeRestoreFromRestorePoint(RestorePointExtra restorePointExtra, string pathToRestore = null)
        {
            if (!RestorePointExtras.Contains(restorePointExtra))
                throw new BackupsExtraException("Backup job does not contain this restore point");
            MakeLogs("Restore operation from restore point done");
            BackupsManager.SaveState();
            RepositoryExtra.MakeRestore(restorePointExtra, pathToRestore);
        }

        public void MakeLogs(string message)
        {
            Log.MakeLog(message ?? throw new BackupsExtraException("log message cannot be null"));
        }
    }
}