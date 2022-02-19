// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class NameSurnamePatronymic
    {
        public NameSurnamePatronymic(string surname, string name, string patronymic)
        {
            Surname = surname ?? throw new BanksException("Surname cannot be null");
            Name = name ?? throw new BanksException("Name cannot be null");
            Patronymic = patronymic ?? throw new BanksException("Patronymic cannot be null");
        }

        public string Name
        {
            get;
        }

        public string Surname
        {
            get;
        }

        public string Patronymic
        {
            get;
        }

        public override string ToString()
        {
            return Surname + " " + Name + " " + Patronymic;
        }
    }
}