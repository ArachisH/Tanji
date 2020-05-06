using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Tanji.Utilities
{
    public static class TResources
    {
        public static Stream GetEmbedResourceStream(string embeddedFileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(s => s.EndsWith(embeddedFileName, StringComparison.CurrentCultureIgnoreCase));

            if (string.IsNullOrEmpty(resourceName))
                throw new ArgumentException("Could not find specified resource name from the manifest.", nameof(embeddedFileName));

            return assembly.GetManifestResourceStream(resourceName)
                ?? throw new InvalidOperationException("Could not load manifest resource stream.");
        }
    }
}
