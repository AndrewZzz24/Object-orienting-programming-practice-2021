// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Backups.Entities;

namespace Backups.Services
{
    public interface IBackupJob
    {
        RestorePoint MakeRestorePoint();
        BackupJob AddJobObjectsToBackupJob(params JobObject[] jobObject);
        BackupJob RemoveJobObjectFromBackupJob(JobObject jobObject);

        List<RestorePoint> GetRestorePoints();

        List<Storage> GetStorages();
    }
}