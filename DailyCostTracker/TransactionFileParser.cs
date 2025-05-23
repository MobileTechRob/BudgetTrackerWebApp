using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatabaseManager;
using DatabaseManager.DataModels;

namespace DailyCostTracker
{
    public class TransactionFileParser
    {
        string fileToParse;
        public DatabaseManager.DataModels.DailyTransaction Parser(string lineFromfile)
        {
            //POSTED DATE, DESCRIPTION, AMOUNT, CURRENCY, TRANSACTION REFERENCE NUMBER, FI TRANSACTION REFERENCE, TYPE, CREDIT/ DEBIT,ORIGINAL AMOUNT
            //04 / 24 / 2025,PPD MSPBNA BANK TRANSFER,-100.00,USD,,2025042400001,DDA Debit, Debit,
            string[] fields = lineFromfile.Split(',');

            DatabaseManager.DataModels.DailyTransaction dailyAccountTransaction = new DatabaseManager.DataModels.DailyTransaction();
            dailyAccountTransaction.Posted_Date = DateTime.Parse(fields[0]);
            dailyAccountTransaction.Description = fields[1];
            dailyAccountTransaction.Amount = Decimal.Parse(fields[2]);
            dailyAccountTransaction.Currency = fields[3];
           
            long trn;
            //if (long.TryParse(fields[4], out trn))
                dailyAccountTransaction.Transaction_Reference_Number = fields[4];

            long fitrn;
            //if (long.TryParse(fields[5], out fitrn))
                dailyAccountTransaction.Fi_Transaction_Reference = fields[5];

            dailyAccountTransaction.Type = fields[6];
            dailyAccountTransaction.Credit_Debit = fields[7];
            dailyAccountTransaction.Original_Amount = 0m;           
            dailyAccountTransaction.Content = string.Join("", fields);

            return dailyAccountTransaction;
        }       
    }
}
