// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Entities;
using BackupsExtra.Services.RestorePointLimits;

namespace BackupsExtra.Services.RestorePointsLimitsCombination
{
    public class AllLimitsCarriedOut : ILimitsCombination
    {
        public List<RestorePointExtra> CombineLimits(
            List<RestorePointExtra> restorePointsExtra,
            List<IRestorePointLimit> restorePointLimits,
            RepositoryExtra repositoryExtra)
        {
            HashSet<RestorePointExtra> pointsToDelete = GetPointsToDelete(restorePointsExtra, restorePointLimits);

            return restorePointsExtra.Where(restorePointExtra => !pointsToDelete.Contains(restorePointExtra)).ToList();
        }

        public HashSet<RestorePointExtra> GetPointsToDelete(
            List<RestorePointExtra> restorePointExtras,
            List<IRestorePointLimit> restorePointLimits)
        {
            var pointsToDelete = new HashSet<RestorePointExtra>();
            foreach (IRestorePointLimit limit in restorePointLimits)
            {
                pointsToDelete.UnionWith(limit.GetRestorePointsToDelete(restorePointExtras));
            }

            return pointsToDelete;
        }
    }
}