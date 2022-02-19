// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class PersonBuilder
    {
        private NameSurnamePatronymic _fullName = null;

        private Address _address = null;

        private Passport _passport = null;

        public PersonBuilder SetFullName(NameSurnamePatronymic fullName)
        {
            _fullName = fullName ?? throw new BanksException("Full name cannot be null");
            return this;
        }

        public PersonBuilder SetAddress(Address address)
        {
            _address = address ?? throw new BanksException("Setting address cannot be null");
            return this;
        }

        public PersonBuilder SetPassport(Passport passport)
        {
            _passport = passport ?? throw new BanksException("Setting passport cannot be null");
            return this;
        }

        public Person GetPerson()
        {
            return new Person(_fullName, _address, _passport);
        }
    }
}