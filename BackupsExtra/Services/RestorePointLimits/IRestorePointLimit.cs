// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using BackupsExtra.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Services.RestorePointLimits
{
    public interface IRestorePointLimit
    {
        List<RestorePointExtra> DeleteRestorePointsOutOfLimit(
            List<RestorePointExtra> restorePoints,
            RepositoryExtra repositoryExtra);

        List<RestorePointExtra> Merge(List<RestorePointExtra> restorePointExtras, RepositoryExtra repositoryExtra)
        {
            HashSet<RestorePointExtra> pointToDelete = GetRestorePointsToDelete(restorePointExtras);
            RestorePointExtra newestRestorePoint = null;

            foreach (RestorePointExtra restorePoint in restorePointExtras)
            {
                if (!pointToDelete.Contains(restorePoint) &&
                    (newestRestorePoint == null || restorePoint.DateTime < newestRestorePoint.DateTime))
                    newestRestorePoint = restorePoint;
            }

            if (newestRestorePoint == null)
                throw new BackupsExtraException("newest Restore Point is null");

            List<Storage> newestRpStorages = newestRestorePoint.RestorePoint.GetStorages();

            foreach (RestorePointExtra deletingRestorePoint in pointToDelete)
            {
                foreach (Storage storage in deletingRestorePoint.RestorePoint.GetStorages())
                {
                    var jobObjectsToAdd = new List<JobObject>();
                    foreach (JobObject jobObject in storage.GetJobObjects())
                    {
                        bool isInNewRp = newestRpStorages.Any(newestRpStorage =>
                            newestRpStorage.GetJobObjects().Contains(jobObject));

                        if (!isInNewRp)
                        {
                            jobObjectsToAdd.Add(jobObject);
                        }
                    }

                    if (jobObjectsToAdd.Count != 0)
                        newestRpStorages.Add(new Storage(jobObjectsToAdd.ToArray()));
                }
            }

            newestRestorePoint.RestorePoint.Storages = newestRpStorages;
            var result = restorePointExtras.Where(restorePoint => !pointToDelete.Contains(restorePoint)).ToList();
            return result;
        }

        HashSet<RestorePointExtra> GetRestorePointsToDelete(List<RestorePointExtra> restorePoints);
    }
}