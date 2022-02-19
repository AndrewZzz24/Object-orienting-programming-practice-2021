// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Backups.Entities;
using Backups.Services;

namespace Backups.Tools
{
    public class SingleRestorePointCreationAlgorithm : IRestorePointCreationAlgorithm
    {
        public RestorePoint CreateRestorePoint(List<JobObject> jobObjects, Repository repository, bool keepInMemory)
        {
            var storage = new Storage(jobObjects.ToArray());
            var restorePoint = new RestorePoint(new List<Storage>() { storage });

            if (!keepInMemory)
                repository.CreateRestorePoint(restorePoint);

            return restorePoint;
        }
    }
}