// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using System.Collections.Generic;
using Banks.Tools;

namespace Banks.Entities
{
    public class DepositInterestRate
    {
        public DepositInterestRate(List<AmountPercent> interestRates)
        {
            CheckInputDataCorrectness(interestRates);
            InterestRates = interestRates ?? throw new BanksException("interest rate list cannot be null");
        }

        public List<AmountPercent> InterestRates
        {
            get;
        }

        private void CheckInputDataCorrectness(List<AmountPercent> interestRates)
        {
            for (int i = 1; i < interestRates.Count; i++)
            {
                if (interestRates[i].Amount <= interestRates[i - 1].Amount
                    || interestRates[i].Percent <= interestRates[i - 1].Percent)
                    throw new BanksException("Invalid interestRates");
            }
        }
    }
}