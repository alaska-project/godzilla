using Godzilla.AspNetCore.Ui.Middlewares;
using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.AspNetCore.Ui.Model
{
    internal class StaticRedirect : IRedirect
    {
        public StaticRedirect(string from, string to)
        {
            From = from;
            To = to;
        }

        public string From { get; }

        public string To { get; }
    }
}
