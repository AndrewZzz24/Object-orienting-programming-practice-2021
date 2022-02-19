// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using BackupsExtra.Entities;
using BackupsExtra.Services.RestorePointLimits;

namespace BackupsExtra.Services.RestorePointsDeletingTypes
{
    public interface IRestorePointCleanType
    {
        List<RestorePointExtra> Evaluate(
            IRestorePointLimit restorePointLimit,
            List<RestorePointExtra> restorePoint,
            RepositoryExtra repositoryExtra);
    }
}