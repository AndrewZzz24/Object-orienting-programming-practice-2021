// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;

namespace Backups.Entities
{
    public class JobObject : ICloneable
    {
        public JobObject(string name)
        {
            FileFullName = name;
            Name = GetFileNameFromPath(name);
        }

        public string Name
        {
            get;
        }

        public string FileFullName
        {
            get;
        }

        public object Clone()
        {
            return new JobObject(Name);
        }

        private static string GetFileNameFromPath(string name)
        {
            return name.Substring(name.LastIndexOf('/') + 1, name.Length - name.LastIndexOf('/') - 1);
        }
    }
}