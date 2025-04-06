using Bulge.Properties;
using Bulge.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulge
{
    public class CategoryIcon : Grasshopper.Kernel.GH_AssemblyPriority
    {
        public override Grasshopper.Kernel.GH_LoadingInstruction PriorityLoad()
        {
            Grasshopper.Instances.ComponentServer.AddCategoryIcon("Bulge", Resources.IconBulge);
            Grasshopper.Instances.ComponentServer.AddCategorySymbolName("Bulge", 'B');
            return Grasshopper.Kernel.GH_LoadingInstruction.Proceed;
        }
    }
}
