﻿using System;
using System.IO;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        const int MaxTollCharge = 60;

        public void Run(IFile file, string filePath)
        {
            DateTime[] dates = GetDatesFromFile(file, filePath);
            Console.Write("The total fee for the inputfile is " + CalculateTotalTollFeeCost(dates));
        }

        DateTime[] GetDatesFromFile(IFile file, string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException();
            string fileData = file.ReadAllText(filePath);
            string[] dateStrings = fileData.Split(", ");
            DateTime[] dates = new DateTime[dateStrings.Length];
            for (int i = 0; i < dates.Length; i++)
            {
                if (!DateTime.TryParse(dateStrings[i], out dates[i]))
                    throw new FormatException();
            }
            return dates;
        }

        int CalculateTotalTollFeeCost(DateTime[] dates)
        {
            int fee = 0;
            int highestFeeWithinHour = 0;
            DateTime compareDate = dates[0];
            for (int i = 0; i < dates.Length; i++)
            {
                double diffInMinutes = (dates[i] - compareDate).TotalMinutes;
                if (diffInMinutes > 60)
                {
                    fee += highestFeeWithinHour;
                    highestFeeWithinHour = CalculateTollFee(dates[i]);
                    compareDate = dates[i];
                }
                else
                {
                    int currentDateFee = CalculateTollFee(dates[i]);
                    highestFeeWithinHour = Math.Max(currentDateFee, highestFeeWithinHour);
                }
                if (i == dates.Length - 1)
                    fee += highestFeeWithinHour;
            }
            return Math.Clamp(fee, 0, MaxTollCharge);
        }

        public int CalculateTollFee(DateTime date)
        {
            int tollFee;
            int hour = date.Hour;
            int minute = date.Minute;

            if (IsDayTollFree(date))
                tollFee = 0;
            if (hour == 6 && minute >= 0 && minute <= 29)
                tollFee = 8;
            else if (hour == 6 && minute >= 30 && minute <= 59)
                tollFee = 13;
            else if (hour == 7 && minute >= 0 && minute <= 59)
                tollFee = 18;
            else if (hour == 8 && minute >= 0 && minute <= 29)
                tollFee = 13;
            else if (hour == 8 && minute >= 30 && minute <= 59)
                tollFee = 8;
            else if (hour >= 9 && hour <= 14)
                tollFee = 8;
            else if (hour == 15 && minute >= 0 && minute <= 29)
                tollFee = 13;
            else if ( (hour == 15 && minute >= 30) || (hour == 16 && minute <= 59) )
                tollFee = 18;
            else if (hour == 17 && minute >= 0 && minute <= 59)
                tollFee = 13;
            else if (hour == 18 && minute >= 0 && minute <= 29)
                tollFee = 8;
            else tollFee = 0;

            return tollFee;
        }

        public bool IsDayTollFree(DateTime day)
        {
            bool isTollFree = day.DayOfWeek == DayOfWeek.Saturday ||
                day.DayOfWeek == DayOfWeek.Sunday ||
                day.Month == 7;
            return isTollFree;
        }
    }
}
