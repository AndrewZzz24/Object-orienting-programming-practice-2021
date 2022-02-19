// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Backups.Entities;

namespace BackupsExtra.Entities
{
    public class RestorePointExtra
    {
        public RestorePointExtra(RestorePoint restorePoint, DateTime dateTime)
        {
            RestorePoint = restorePoint;
            DateTime = dateTime;
        }

        public RestorePoint RestorePoint
        {
            get;
        }

        public DateTime DateTime
        {
            get;
        }
    }
}