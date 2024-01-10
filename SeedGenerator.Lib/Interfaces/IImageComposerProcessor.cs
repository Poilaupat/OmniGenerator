using SeedGenerator.Lib.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedGenerator.Lib.Interfaces
{
    public interface IImageComposerProcessor
    {
        Task ProcessAsync(Root root);
    }
}
