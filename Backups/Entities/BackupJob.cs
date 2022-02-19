// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Backups.Services;
using Backups.Tools;

namespace Backups.Entities
{
    public class BackupJob : IBackupJob
    {
        private readonly List<JobObject> _jobObjects = new List<JobObject>();
        private List<RestorePoint> _restorePoints = new List<RestorePoint>();

        private IRestorePointCreationAlgorithm _restorePointCreationAlgorithm;

        // private string _saveDirectory;
        private StorageType _storageType;

        public BackupJob()
        {
        }

        public BackupJob(
            string name,
            IRestorePointCreationAlgorithm restorePointCreationAlgorithm,
            Repository repository,
            StorageType storageType)
        {
            BackupClassInitializer(name, restorePointCreationAlgorithm, repository, storageType);
        }

        public BackupJob(
            string name,
            IRestorePointCreationAlgorithm restorePointCreationAlgorithm,
            Repository repository)
        {
            BackupClassInitializer(name, restorePointCreationAlgorithm, repository, StorageType.SplitStorage);
        }

        public StorageType StorageType
        {
            get => _storageType;
            set
            {
                _storageType = value;
                _restorePointCreationAlgorithm = _storageType switch
                {
                    StorageType.SingleStorage => new SingleRestorePointCreationAlgorithm(),
                    StorageType.SplitStorage => new SplitRestorePointCreationAlgorithm(),
                    _ => _restorePointCreationAlgorithm
                };
            }
        }

        public string Name
        {
            get;
            private set;
        }

        public Repository Repository
        {
            get;
            private set;
        }

        public bool KeepInMemory
        {
            get;
            set;
        }

        public List<RestorePoint> RestorePoints
        {
            get => _restorePoints;
            set => _restorePoints = value ?? throw new Exception();
        }

        public RestorePoint MakeRestorePoint()
        {
            RestorePoint restorePoint =
                _restorePointCreationAlgorithm.CreateRestorePoint(_jobObjects, Repository, KeepInMemory);
            _restorePoints.Add(restorePoint);
            return restorePoint;
        }

        public BackupJob AddJobObjectsToBackupJob(params JobObject[] jobObject)
        {
            foreach (JobObject jo in jobObject)
            {
                if (jo == null)
                    throw new BackupException("JobObject cannot be null");
                _jobObjects.Add(jo);
            }

            return this;
        }

        public BackupJob RemoveJobObjectFromBackupJob(JobObject jobObject)
        {
            if (!_jobObjects.Contains(jobObject))
                throw new BackupException("Backup does not contain " + jobObject.Name + " job object");
            _jobObjects.Remove(jobObject);

            return this;
        }

        public List<RestorePoint> GetRestorePoints()
        {
            return _restorePoints.ToList();
        }

        public List<Storage> GetStorages()
        {
            var result = new List<Storage>();
            foreach (RestorePoint rp in _restorePoints)
            {
                result.AddRange(rp.GetStorages());
            }

            return result;
        }

        private void BackupClassInitializer(
            string name,
            IRestorePointCreationAlgorithm restorePointCreationAlgorithm,
            Repository repository,
            StorageType storageType)
        {
            StorageType = storageType;
            _restorePointCreationAlgorithm = restorePointCreationAlgorithm ?? throw new NullReferenceException();
            Name = name ?? throw new NullReferenceException();
            KeepInMemory = true;
            Repository = repository;
        }
    }
}