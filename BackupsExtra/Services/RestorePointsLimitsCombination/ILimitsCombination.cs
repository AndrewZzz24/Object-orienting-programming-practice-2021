// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using BackupsExtra.Entities;
using BackupsExtra.Services.RestorePointLimits;

namespace BackupsExtra.Services.RestorePointsLimitsCombination
{
    public interface ILimitsCombination
    {
        List<RestorePointExtra> CombineLimits(
            List<RestorePointExtra> restorePointsExtra,
            List<IRestorePointLimit> restorePointLimits,
            RepositoryExtra repositoryExtra);

        HashSet<RestorePointExtra> GetPointsToDelete(
            List<RestorePointExtra> restorePointExtras,
            List<IRestorePointLimit> restorePointLimits);
    }
}