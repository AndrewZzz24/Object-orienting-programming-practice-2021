// @author Andrew Zmushko (andrewzmushko@gmail.com)

using System;
using Banks.Tools;

namespace Banks.Entities
{
    public class CentralBankTimeLine
    {
        private const int DAY = 1;
        private const int MONTH = 30;
        private const int YEAR = 365;

        public CentralBankTimeLine(CentralBank centralBank)
        {
            CentralBank = centralBank ?? throw new BanksException("Central bank cannot be null");
            CurrentDay = DAY;
        }

        public CentralBank CentralBank
        {
            get;
        }

        public int CurrentDay
        {
            get;
            private set;
        }

        public int NextDay()
        {
            MakeBankOperations();
            return CurrentDay;
        }

        public int NextMonth()
        {
            for (int i = DAY; i < MONTH; i++)
            {
                MakeBankOperations();
            }

            return CurrentDay;
        }

        public int NextYear()
        {
            for (int i = DAY; i < YEAR; i++)
            {
                MakeBankOperations();
            }

            return CurrentDay;
        }

        private void MakeBankOperations()
        {
            CentralBank.CountFee();
            CurrentDay++;
            if (CurrentDay == MONTH)
            {
                CentralBank.ChargeFee();
                CurrentDay = DAY;
            }
        }
    }
}