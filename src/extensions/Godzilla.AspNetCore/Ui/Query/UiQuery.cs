using Godzilla.AspNetCore.Ui.Abstractions;
using Godzilla.AspNetCore.Ui.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.AspNetCore.Ui.Query
{
    internal class UiQuery : IUiQuery
    {
        public IEnumerable<UiEntityContextReference> GetContext()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UiNodeReference> GetRootNodes(string contextId)
        {
            throw new NotImplementedException();
        }
    }
}
