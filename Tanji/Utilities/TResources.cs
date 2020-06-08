using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Reflection;

namespace Tanji.Utilities
{
    public static class TResources
    {
        public static Image Avatar { get; }
        public static Icon Tanji_256 { get; }

        static TResources()
        {
            Tanji_256 = new Icon(GetEmbedResourceStream("Tanji_256.ico"));
            Avatar = Image.FromStream(GetEmbedResourceStream("Avatar.png"));
        }

        private static Stream GetEmbedResourceStream(string embeddedFileName)
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