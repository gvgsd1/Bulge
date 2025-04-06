using System;
using System.Drawing;
using System.Reflection;
using Bulge.Properties;
using Grasshopper;
using Grasshopper.Kernel;

namespace Bulge
{
    public class BulgeInfo : GH_AssemblyInfo
    {
        public override string Name => "Bulge";

        public override Bitmap Icon => Resources.IconBulge;

        public override string Description => "A Magic Pocket with Everything You Need";

        public override Guid Id => new Guid("4cbd5c2f-7baa-4212-87c3-d1ddd3255cf4");

        public override string AuthorName => "Gareth Volka";

        public override string AuthorContact => "garethvolka@gmail.com";

        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
    }
}