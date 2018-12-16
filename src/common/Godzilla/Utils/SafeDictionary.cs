using System;
using System.Collections.Generic;
using System.Text;

namespace Godzilla.Utils
{
    internal class SafeDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _innerDict = new Dictionary<TKey, TValue>();

        public IEnumerable<TValue> Values => _innerDict.Values;

        public IEnumerable<TKey> Keys => _innerDict.Keys;

        public TValue Retreive(TKey key, Func<TValue> valueProvider)
        {
            if (_innerDict.ContainsKey(key))
                return _innerDict[key];

            lock (this)
            {
                if (_innerDict.ContainsKey(key))
                    return _innerDict[key];

                var value = valueProvider();
                _innerDict[key] = value;
                return value;
            }
        }
    }
}
