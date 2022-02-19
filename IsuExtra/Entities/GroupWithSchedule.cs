// @author Andrew Zmushko (andrewzmushko@gmail.com)

using Isu;

namespace IsuExtra.Entities
{
    public class GroupWithSchedule
    {
        public GroupWithSchedule(Group @group)
        {
            Group = group;
            Schedule = new Schedule();
        }

        public GroupWithSchedule(Group @group, Schedule schedule)
        {
            Group = group;
            Schedule = schedule;
        }

        public Group Group
        {
            get;
        }

        public Schedule Schedule
        {
            get;
        }
    }
}