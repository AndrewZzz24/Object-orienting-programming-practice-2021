// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Banks.Tools;

namespace Banks.Entities
{
    public class Passport
    {
        public Passport(string passportNumber)
        {
            CheckPassportCorrectness(passportNumber);
            PassportNumber = passportNumber;
        }

        public string PassportNumber
        {
            get;
        }

        public override string ToString()
        {
            return PassportNumber;
        }

        private void CheckPassportCorrectness(string passportNum)
        {
            if (passportNum == null)
                throw new BanksException("Passport cannot be null");
            if (!passportNum.All(char.IsDigit))
                throw new BanksException("Invalid passport number");
        }
    }
}