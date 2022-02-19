// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System.Collections.Generic;
using Backups.Entities;

namespace Backups.Services
{
    public interface IRestorePointCreationAlgorithm
    {
        RestorePoint CreateRestorePoint(List<JobObject> jobObjects, Repository repository, bool keepInMemory);
    }
}