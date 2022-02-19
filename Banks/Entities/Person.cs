// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class Person
    {
        public Person(NameSurnamePatronymic fullName, Address address, Passport passport)
        {
            FullName = fullName ?? throw new BanksException("Full name cannot be null");
            Address = address;
            Passport = passport;
        }

        public NameSurnamePatronymic FullName
        {
            get;
        }

        public Address Address
        {
            get;
            set;
        }

        public Passport Passport
        {
            get;
            set;
        }

        public static PersonBuilder Builder(NameSurnamePatronymic fullName)
        {
            return new PersonBuilder().SetFullName(fullName);
        }

        public override string ToString()
        {
            return FullName.ToString() + Address.ToString() + Passport.ToString();
        }
    }
}