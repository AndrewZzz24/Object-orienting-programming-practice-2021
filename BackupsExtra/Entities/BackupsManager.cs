// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using BackupsExtra.Services.Logging;
using BackupsExtra.Services.RestorePointLimits;
using BackupsExtra.Services.RestorePointsDeletingTypes;
using BackupsExtra.Services.RestorePointsLimitsCombination;
using Newtonsoft.Json;

namespace BackupsExtra.Entities
{
    public class BackupsManager
    {
        public BackupsManager(string configFile)
        {
            ConfigFile = configFile;
            BackupJobsExtra = new List<BackupJobExtra>();
        }

        public List<BackupJobExtra> BackupJobsExtra
        {
            get;
            set;
        }

        public string ConfigFile
        {
            get;
        }

        public BackupJobExtra AddBackupJobExtra(
            BackupJob backupJob,
            ILimitsCombination limitsCombination,
            IBackupsLog backupsLog)
        {
            var result = new BackupJobExtra(backupJob, limitsCombination, backupsLog, this);
            BackupJobsExtra.Add(result);
            SaveState();
            return result;
        }

        public List<BackupJobExtra> UpdateState()
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };

            string text = File.ReadAllText(ConfigFile);
            BackupJobsExtra = JsonConvert.DeserializeObject<List<BackupJobExtra>>(text, serializerSettings);

            return BackupJobsExtra;
        }

        public void CleanRestorePointsList(
            BackupJobExtra backupJobExtra,
            IRestorePointLimit restorePointLimit,
            IRestorePointCleanType cleanType)
        {
            backupJobExtra.CleanRestorePointsList(restorePointLimit, cleanType);
        }

        public void MakeRestoreFromRestorePoint(BackupJobExtra backupJobExtra, RestorePointExtra restorePointExtra)
        {
            backupJobExtra.MakeRestoreFromRestorePoint(restorePointExtra);
        }

        public void SaveState()
        {
            if (ConfigFile == null)
                return;
            File.Create(ConfigFile).Close();
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All,
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            };
            File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(BackupJobsExtra, Formatting.Indented, serializerSettings));
        }
    }
}