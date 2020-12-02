using System;
using TollFeeCalculator;

namespace TollFeeCalculatorTests
{
    class MockFile : IFile
    {
        public string ReadAllText(string output)
        {
            return output;
        }
    }
}
