using System;

namespace TollFeeCalculator
{
    public class TollCalculator
    {
        static void Main()
        {
            Run(Environment.CurrentDirectory + "../../../../testData.txt");
        }

        public static void Run(string inputFile)
        {
            var timeStamps = GetTimeStamps(inputFile);
            Console.Write("The total fee for the inputfile is " + CalculateTotalTollFeeCost(timeStamps));
        }

        static DateTime[] GetTimeStamps(string inputFile)
        {
            string inData = System.IO.File.ReadAllText(inputFile);
            string[] dateStrings = inData.Split(", ");
            DateTime[] dates = new DateTime[dateStrings.Length - 1];
            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.Parse(dateStrings[i]);
            }
            return dates;
        }
        static int CalculateTotalTollFeeCost(DateTime[] d)
        {
            int fee = 0;
            DateTime si = d[0]; //Starting interval
            foreach (var d2 in d)
            {
                long diffInMinutes = (d2 - si).Minutes;
                if (diffInMinutes > 60)
                {
                    fee += CalculateTollFee(d2);
                    si = d2;
                }
                else
                {
                    fee += Math.Max(CalculateTollFee(d2), CalculateTollFee(si));
                }
            }
            return Math.Max(fee, 60);
        }

        static int CalculateTollFee(DateTime d)
        {
            if (IsTollFree(d)) return 0;
            int hour = d.Hour;
            int minute = d.Minute;
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }
        //Gets free dates
        static bool IsTollFree(DateTime day)
        {
            return (int)day.DayOfWeek == 5 || (int)day.DayOfWeek == 6 || day.Month == 7;
        }
    }
}
