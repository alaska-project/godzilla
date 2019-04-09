using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Godzilla.AspNetCore.Ui.Services
{
    public class UiNodeConverter
    {
        public string GetItemType(string collectionId)
        {
            return collectionId
                .Split('_')
                .LastOrDefault();
        }
    }
}
