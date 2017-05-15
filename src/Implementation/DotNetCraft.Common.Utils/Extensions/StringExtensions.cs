namespace DotNetCraft.Common.Utils.Extensions
{
    public static class StringExtensions
    {
        public static string GetPart(this string input, int maxSize)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            if (input.Length < maxSize)
                return input;

            string str = input.Substring(0, maxSize);
            string result = string.Format("{0}...", str);
            return result;
        }
    }
}
