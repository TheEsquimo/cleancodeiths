using System;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        const int maxTollCharge = 60;
        const string testDataPath = "../../../../testData.txt";

        static void Main()
        {
            Run(new File(), Environment.CurrentDirectory + testDataPath);
        }

        public static void Run(IFile file, string inputFile)
        {
            var timeStamps = GetDatesFromFile(file, inputFile);
            Console.Write("The total fee for the inputfile is " + CalculateTotalTollFeeCost(timeStamps));
        }

        static DateTime[] GetDatesFromFile(IFile file, string inputFile)
        {
            string fileData = file.ReadAllText(inputFile);
            string[] dateStrings = fileData.Split(", ");
            DateTime[] dates = new DateTime[dateStrings.Length];
            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.Parse(dateStrings[i]);
            }
            return dates;
        }

        static int CalculateTotalTollFeeCost(DateTime[] dates)
        {
            int fee = 0;
            int highestFeeWithinHour = 0;
            DateTime currentCompareDate = dates[0];
            for (int i = 0; i < dates.Length; i++)
            {
                double diffInMinutes = (dates[i] - currentCompareDate).TotalMinutes;
                if (diffInMinutes > 60)
                {
                    fee += highestFeeWithinHour;
                    highestFeeWithinHour = CalculateTollFee(dates[i]);
                    currentCompareDate = dates[i];
                    if (i == dates.Length - 1)
                        fee += highestFeeWithinHour;
                }
                else
                {
                    highestFeeWithinHour = Math.Max(CalculateTollFee(dates[i]), CalculateTollFee(currentCompareDate));
                    if (i == dates.Length - 1)
                        fee += highestFeeWithinHour;
                }
            }
            return Math.Clamp(fee, 0, maxTollCharge);
        }

        static int CalculateTollFee(DateTime date)
        {
            int tollFee;
            int hour = date.Hour;
            int minute = date.Minute;

            if (IsTollFree(date))
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

        static bool IsTollFree(DateTime day)
        {
            bool isTollFree = day.DayOfWeek == DayOfWeek.Saturday ||
                day.DayOfWeek == DayOfWeek.Sunday ||
                day.Month == 7;
            return isTollFree;
        }
    }
}
