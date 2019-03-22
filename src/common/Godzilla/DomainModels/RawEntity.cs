using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.DomainModels
{
    public class RawEntity
    {
        private string _rawValue;

        public RawEntity(string rawValue)
        {
            _rawValue = rawValue;
        }

        public string RawValue => _rawValue;
    }
}
