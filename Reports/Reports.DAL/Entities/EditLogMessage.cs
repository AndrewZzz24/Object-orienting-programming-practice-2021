// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;

namespace Reports.DAL.Entities
{
    public class EditLogMessage
    {
        public EditLogMessage()
        {
        }

        public EditLogMessage(Employee editor, string editionDetails)
        {
            Editor = editor;
            EditionDetails = editionDetails;
            Time = DateTime.Now;
        }

        public string EditionDetails
        {
            get;
        }

        public Employee Editor
        {
            get;
        }

        public DateTime Time
        {
            get;
        }
    }
}