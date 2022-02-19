// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using BackupsExtra.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Services.RestorePointLimits
{
    public class AmountRestorePointLimit : IRestorePointLimit
    {
        private const int LowestPossibleNumOfRestorePoints = 0;

        public AmountRestorePointLimit(int maximumNumberOfRestorePoints)
        {
            if (maximumNumberOfRestorePoints < LowestPossibleNumOfRestorePoints)
                throw new BackupsExtraException("Number of points cannot be less than zero");
            MaximumNumberOfRestorePoints = maximumNumberOfRestorePoints;
        }

        public int MaximumNumberOfRestorePoints
        {
            get;
        }

        public List<RestorePointExtra> DeleteRestorePointsOutOfLimit(
            List<RestorePointExtra> restorePoints,
            RepositoryExtra repositoryExtra)
        {
            if (restorePoints == null)
                throw new BackupsExtraException("Restore points cannot be null");

            if (restorePoints.Count <= MaximumNumberOfRestorePoints)
                return restorePoints;

            var result = new List<RestorePointExtra>();

            HashSet<RestorePointExtra> pointsToDelete = GetRestorePointsToDelete(restorePoints);
            foreach (RestorePointExtra restorePoint in restorePoints)
            {
                if (pointsToDelete.Contains(restorePoint))
                {
                    repositoryExtra.DeleteRestorePoint(restorePoint);
                }
                else
                {
                    result.Add(restorePoint);
                }
            }

            if (result.Count == restorePoints.Count)
                throw new BackupsExtraException("Impossible to delete all the restore points");

            return result;
        }

        public HashSet<RestorePointExtra> GetRestorePointsToDelete(List<RestorePointExtra> restorePoints)
        {
            if (restorePoints == null)
                throw new BackupsExtraException("Restore points cannot be null");

            var result = new HashSet<RestorePointExtra>();

            if (MaximumNumberOfRestorePoints > restorePoints.Count)
                return result;

            for (int i = 0; i < restorePoints.Count - MaximumNumberOfRestorePoints; i++)
            {
                result.Add(restorePoints[i]);
            }

            return result;
        }
    }
}