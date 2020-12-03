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
            MockFile mockFile = new MockFile();
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                string threeWithinAnHour = "2020-11-27 06:20, 2020-11-27 06:59, 2020-11-27 07:10";
                string expected = "The total fee for the inputfile is 18";
                TollCalculator.Run(mockFile, threeWithinAnHour);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }

        [TestMethod]
        public void TestAccuracyOfTollPricesByTime()
        {
            MockFile mockFile = new MockFile();
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
                using (StringWriter stringWriter = new StringWriter())
                {
                    Console.SetOut(stringWriter);
                    string expected = $"The total fee for the inputfile is {tollFeeTest.Value}";
                    string tollFeeDate = tollFeeTest.Key.ToString();
                    TollCalculator.Run(mockFile, tollFeeDate);
                    Assert.AreEqual(expected, stringWriter.ToString());
                }
            }
        }

        [TestMethod]
        public void TestFreeTollDayRules()
        {
            MockFile mockFile = new MockFile();
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
                using (StringWriter stringWriter = new StringWriter())
                {
                    Console.SetOut(stringWriter);
                    string expected = $"The total fee for the inputfile is 0";
                    TollCalculator.Run(mockFile, date.ToString());
                    Assert.AreEqual(expected, stringWriter.ToString());
                }
            }
        }

        [TestMethod]
        public void TestMaximumTollCharge()
        {
            MockFile mockFile = new MockFile();
            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                string exceedsDayMaximumToll = "2020-11-27 06:30, 2020-11-27 07:31, 2020-11-27 08:32, 2020-11-27 15:00, 2020-11-27 16:01";
                string expected = "The total fee for the inputfile is 60";
                TollCalculator.Run(mockFile, exceedsDayMaximumToll);
                Assert.AreEqual(expected, stringWriter.ToString());
            }
        }
    }
}
