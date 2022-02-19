// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Tools;

namespace Backups.Entities
{
    public class Storage
    {
        public Storage()
        {
        }

        public Storage(params JobObject[] jobObjects)
        {
            if (jobObjects.Contains(null))
                throw new BackupException("job object cannot be null");
            StoredJobObjects = new List<JobObject>();
            StoredJobObjects.AddRange(jobObjects);
        }

        public List<JobObject> StoredJobObjects
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj.GetType() == typeof(Storage))
            {
                var obj1 = (Storage)obj;

                if (StoredJobObjects.Count != obj1.StoredJobObjects.Count)
                    return false;

                foreach (JobObject jobObject in StoredJobObjects)
                {
                    bool hasEqual = false;

                    foreach (JobObject jb in obj1.StoredJobObjects)
                    {
                        if (jb.FileFullName.Equals(jobObject.FileFullName))
                        {
                            hasEqual = true;
                            break;
                        }
                    }

                    if (!hasEqual)
                        return false;
                }

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StoredJobObjects, Name);
        }

        public List<JobObject> GetJobObjects()
        {
            return StoredJobObjects;
        }
    }
}