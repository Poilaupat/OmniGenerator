using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IRootBuilder
    {
        Root Build(RootParam param);
    }
}
