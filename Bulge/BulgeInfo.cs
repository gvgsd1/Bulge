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

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => Resources.IconBulge;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("4cbd5c2f-7baa-4212-87c3-d1ddd3255cf4");

        //Return a string identifying you or your company.
        public override string AuthorName => "";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";

        //Return a string representing the version.  This returns the same version as the assembly.
        public override string AssemblyVersion => GetType().Assembly.GetName().Version.ToString();
    }
}