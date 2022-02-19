// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using BackupsExtra.Entities;
using BackupsExtra.Services.RestorePointLimits;

namespace BackupsExtra.Services.RestorePointsDeletingTypes
{
    public class MergeRestorePointsCleanType : IRestorePointCleanType
    {
        public List<RestorePointExtra> Evaluate(
            IRestorePointLimit restorePointLimit,
            List<RestorePointExtra> restorePoint,
            RepositoryExtra repositoryExtra)
        {
            return restorePointLimit.Merge(restorePoint, repositoryExtra);
        }
    }
}