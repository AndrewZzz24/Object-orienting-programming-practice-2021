namespace Isu
{
    public class Student
    {
        private string _name;
        private Group @_group;
        private int id;

        public Student(Group @group, string name, int id)
        {
            _name = name;
            this.id = id;
            _group = group;
        }

        public string GetName()
        {
            return _name;
        }

        public Group GetGroup()
        {
            return _group;
        }

        public int GetId()
        {
            return id;
        }

        public void SetGroup(Group @group)
        {
            _group = group;
        }
    }
}