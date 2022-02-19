// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using System.Linq;

namespace Backups.Entities
{
    public class RestorePoint
    {
        public RestorePoint(List<Storage> storages)
        {
            if (storages == null || storages.Contains(null))
                throw new NullReferenceException("storage cannot be null");
            Storages = new List<Storage>();
            Storages.AddRange(storages);
        }

        public List<Storage> Storages
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<Storage> GetStorages()
        {
            return Storages.ToList();
        }
    }
}