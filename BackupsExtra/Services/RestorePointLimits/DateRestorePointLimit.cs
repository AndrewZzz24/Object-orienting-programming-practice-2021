// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.Linq;
using BackupsExtra.Entities;
using BackupsExtra.Tools;

namespace BackupsExtra.Services.RestorePointLimits
{
    public class DateRestorePointLimit : IRestorePointLimit
    {
        public DateRestorePointLimit(DateTime dateLimit)
        {
            DateLimit = dateLimit;
        }

        public DateTime DateLimit
        {
            get;
        }

        public List<RestorePointExtra> DeleteRestorePointsOutOfLimit(
            List<RestorePointExtra> restorePoints,
            RepositoryExtra repositoryExtra)
        {
            if (restorePoints == null)
                throw new BackupsExtraException("restore points list cannot be null");

            var answer = new List<RestorePointExtra>();

            foreach (RestorePointExtra restorePoint in restorePoints)
            {
                if (restorePoint.DateTime > DateLimit)
                {
                    answer.Add(restorePoint);
                }
                else
                {
                    repositoryExtra.DeleteRestorePoint(restorePoint);
                }
            }

            return answer;
        }

        public HashSet<RestorePointExtra> GetRestorePointsToDelete(List<RestorePointExtra> restorePoints)
        {
            var result = new HashSet<RestorePointExtra>();

            foreach (RestorePointExtra restorePoint in restorePoints.Where(restorePoint =>
                restorePoint.DateTime < DateLimit))
            {
                result.Add(restorePoint);
            }

            if (result.Count == restorePoints.Count)
                throw new BackupsExtraException("Impossible to delete all the restore points");

            return result;
        }
    }
}