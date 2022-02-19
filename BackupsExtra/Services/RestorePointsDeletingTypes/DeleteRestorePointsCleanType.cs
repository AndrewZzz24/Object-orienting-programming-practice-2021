// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using BackupsExtra.Entities;
using BackupsExtra.Services.RestorePointLimits;

namespace BackupsExtra.Services.RestorePointsDeletingTypes
{
    public class DeleteRestorePointsCleanType : IRestorePointCleanType
    {
        public List<RestorePointExtra> Evaluate(
            IRestorePointLimit restorePointLimit,
            List<RestorePointExtra> restorePoint,
            RepositoryExtra repositoryExtra)
        {
            return restorePointLimit.DeleteRestorePointsOutOfLimit(restorePoint, repositoryExtra);
        }
    }
}