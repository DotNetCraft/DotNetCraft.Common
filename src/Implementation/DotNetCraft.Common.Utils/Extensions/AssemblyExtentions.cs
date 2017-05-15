using System.IO;
using System.Reflection;

namespace DotNetCraft.Common.Utils.Extensions
{
    public static class AssemblyExtentions
    {
        public static string GetResourceAsString(this Assembly assembly, string path)
        {
            string result;

            using (var stream = assembly.GetManifestResourceStream(path))
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
