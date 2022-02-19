// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using System.Linq;
using Backups.Entities;
using Backups.Services;

namespace Backups.Tools
{
    public class SplitRestorePointCreationAlgorithm : IRestorePointCreationAlgorithm
    {
        public RestorePoint CreateRestorePoint(List<JobObject> jobObjects, Repository repository, bool keepInMemory)
        {
            if (jobObjects.Contains(null))
                throw new BackupException();

            var result = jobObjects.Select(jo => new Storage(jo)).ToList();

            var restorePoint = new RestorePoint(result);

            if (!keepInMemory)
                repository.CreateRestorePoint(restorePoint);

            return restorePoint;
        }
    }
}