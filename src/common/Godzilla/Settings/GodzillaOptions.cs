using Godzilla.Abstractions;
using Godzilla.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Settings
{
    public class GodzillaOptions<TContext> : IGodzillaOptions<TContext>
        where TContext : EntityContext
    {
        public string PathSeparator { get; set; } = "/";
    }
}
