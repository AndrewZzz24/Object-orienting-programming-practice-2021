// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Entities;
using BackupsExtra.Services.RestorePointsLimitsCombination;
using BackupsExtra.Tools;

namespace BackupsExtra.Services.RestorePointLimits
{
    public class HybridRestorePointLimit : IRestorePointLimit
    {
        public HybridRestorePointLimit(
            List<IRestorePointLimit> restorePointLimits,
            ILimitsCombination limitsCombination)
        {
            RestorePointLimits = restorePointLimits ?? throw new BackupsExtraException("Limits cannot be null");
            CheckRestorePointsLimitsCorrectness(restorePointLimits);
            LimitsCombination = limitsCombination ?? throw new BackupsExtraException();
        }

        public List<IRestorePointLimit> RestorePointLimits
        {
            get;
        }

        public ILimitsCombination LimitsCombination
        {
            get;
        }

        public List<RestorePointExtra> DeleteRestorePointsOutOfLimit(List<RestorePointExtra> restorePoints, RepositoryExtra repositoryExtra)
        {
            return LimitsCombination.CombineLimits(restorePoints, RestorePointLimits, repositoryExtra);
        }

        public HashSet<RestorePointExtra> GetRestorePointsToDelete(List<RestorePointExtra> restorePoints)
        {
            return LimitsCombination.GetPointsToDelete(restorePoints, RestorePointLimits);
        }

        private void CheckRestorePointsLimitsCorrectness(List<IRestorePointLimit> restorePointLimits)
        {
            if (restorePointLimits.Any(restorePointLimit =>
                restorePointLimit.GetType() == typeof(HybridRestorePointLimit)))
            {
                throw new BackupsExtraException("ListOfLimitsCannotContainHybridType");
            }
        }
    }
}