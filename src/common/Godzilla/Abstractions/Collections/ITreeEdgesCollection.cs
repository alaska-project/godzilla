using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Abstractions.Collections
{
    internal interface ITreeEdgesCollection
    {
        bool NodeExists(Guid nodeId);
    }
}
