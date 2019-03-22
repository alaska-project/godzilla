using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.AspNetCore.Ui.Model
{
    public class UiNodeReference
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; }
    }
}
