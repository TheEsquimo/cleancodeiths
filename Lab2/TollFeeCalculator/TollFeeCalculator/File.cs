namespace TollFeeCalculator
{
    class File : IFile
    {
        public string ReadAllText(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
    }
}
