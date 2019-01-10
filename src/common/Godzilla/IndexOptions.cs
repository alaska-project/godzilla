using Godzilla.Abstractions.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla
{
    public class IndexOptions : IIndexOptions
    {
        public bool Unique { get; set; }
    }
}
