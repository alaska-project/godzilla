using Godzilla.AspNetCore.Ui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.AspNetCore.Ui.Abstractions
{
    internal interface IUiQuery
    {
        IEnumerable<UiEntityContextReference> GetContext();
        IEnumerable<UiNodeReference> GetRootNodes(string contextId);
    }
}
