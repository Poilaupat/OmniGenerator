using SeedGenerator.Lib.Data;
using SeedGenerator.Lib.Param;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IRootBuilder
    {
        Root Build(RootParam param);
    }
}
