// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Linq;
using Banks.Tools;

namespace Banks.Entities
{
    public class Address
    {
        public Address(string aFullAddress)
        {
            CheckAddressCorrectness(aFullAddress);
            FullAddress = aFullAddress;
        }

        public string FullAddress
        {
            get;
        }

        public override string ToString()
        {
            return FullAddress;
        }

        private void CheckAddressCorrectness(string address)
        {
            if (address == null)
                throw new BanksException("address cannot be null");
        }
    }
}