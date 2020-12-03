using System;

namespace TollFeeCalculator
{
    class Program
    {
        const string testDataPath = "../../../../testData.txt";

        static void Main()
        {
            TollCalculator tollCalculator = new TollCalculator();
            tollCalculator.Run(new File(), Environment.CurrentDirectory + testDataPath);
        }
    }
}
