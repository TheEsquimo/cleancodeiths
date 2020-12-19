using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    [TestClass]
    public class CalculateTollFeeTests
    {
        [TestMethod]
        public void TestHighestTollFeeWithinHourRule()
        {
            TollCalculator tollCalculator = new TollCalculator();
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                string datesWithinAnHour = Path.GetFullPath(Path.Combine(
                    Environment.CurrentDirectory, "..", "..", "..", "MockingTestData", "datesWithinAnHour.txt"));
                string expected = "The total fee for the inputfile is 18";
                tollCalculator.Run(new TollFeeCalculator.File(), datesWithinAnHour);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void TestAccuracyOfTollPricesByTime()
        {
            TollCalculator tollCalculator = new TollCalculator();
            Dictionary<DateTime, int> tollFeeTestKeys = new Dictionary<DateTime, int>
            {
                { new DateTime(2020, 11, 27, 6, 0, 0), 8 },
                { new DateTime(2020, 11, 27, 6, 29, 0), 8 },
                { new DateTime(2020, 11, 27, 6, 30, 0), 13 },
                { new DateTime(2020, 11, 27, 6, 59, 0), 13 },
                { new DateTime(2020, 11, 27, 7, 0, 0), 18 },
                { new DateTime(2020, 11, 27, 7, 59, 0), 18 },
                { new DateTime(2020, 11, 27, 8, 0, 0), 13 },
                { new DateTime(2020, 11, 27, 8, 29, 0), 13 },
                { new DateTime(2020, 11, 27, 8, 30, 0), 8 },
                { new DateTime(2020, 11, 27, 14, 59, 0), 8 },
                { new DateTime(2020, 11, 27, 15, 0, 0), 13 },
                { new DateTime(2020, 11, 27, 15, 29, 0), 13 },
                { new DateTime(2020, 11, 27, 15, 30, 0), 18 },
                { new DateTime(2020, 11, 27, 16, 59, 0), 18 },
                { new DateTime(2020, 11, 27, 17, 0, 0), 13 },
                { new DateTime(2020, 11, 27, 17, 59, 0), 13 },
                { new DateTime(2020, 11, 27, 18, 0, 0), 8 },
                { new DateTime(2020, 11, 27, 18, 29, 0), 8 },
                { new DateTime(2020, 11, 27, 18, 30, 0), 0 },
                { new DateTime(2020, 11, 27, 5, 59, 0), 0 }
            };

            foreach (var tollFeeTest in tollFeeTestKeys)
            {
                    int expected = tollFeeTest.Value;
                    DateTime tollFeeDate = tollFeeTest.Key;
                    int result = tollCalculator.CalculateTollFee(tollFeeDate);
                    Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public void TestFreeTollDayRules()
        {
            TollCalculator tollCalculator = new TollCalculator();
            DateTime saturday = new DateTime(2020, 12, 5);
            DateTime sunday = new DateTime(2020, 12, 6);
            DateTime firstDayJuly = new DateTime(2020, 7, 1);
            DateTime lastDayJuly = new DateTime(2020, 7, 31);

            DateTime[] freeTollDates = new DateTime[]
            {
                saturday,
                sunday,
                firstDayJuly,
                lastDayJuly
            };

            foreach (var date in freeTollDates)
            {
                    Assert.AreEqual(true, tollCalculator.IsDayTollFree(date));
            }
        }

        [TestMethod]
        public void TestMaximumTollCharge()
        {
            TollCalculator tollCalculator = new TollCalculator();
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                string exceedsDayMaximumToll = Path.GetFullPath(Path.Combine(
                    Environment.CurrentDirectory, "..", "..", "..", "MockingTestData", "datesExceedingMaximumTollCharge.txt"));
                string expected = "The total fee for the inputfile is 60";
                tollCalculator.Run(new TollFeeCalculator.File(), exceedsDayMaximumToll);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestIncorrectFilePathExceptionHandling()
        {
            TollCalculator tollCalculator = new TollCalculator();
            tollCalculator.Run(new TollFeeCalculator.File(), "notAFilePath");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestIncorrectFormatExceptionHandling()
        {
            TollCalculator tollCalculator = new TollCalculator();
            string incorrectFormattedDates = Path.GetFullPath(Path.Combine(
                    Environment.CurrentDirectory, "..", "..", "..", "MockingTestData", "datesInIncorrectFormat.txt"));
            tollCalculator.Run(new TollFeeCalculator.File(), incorrectFormattedDates);
        }
    }
}