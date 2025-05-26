using System;
using System.Collections.Generic;

namespace FRCWebGL.Data
{
    [Serializable]
    public sealed class SerializableDictionary
    {
        public string[] keys;
        public string[] values;

        public Dictionary<string, string> ToDictionary()
        {
            var itemsDictionary = new Dictionary<string, string>();

            for (int i = 0; i < keys.Length; i++)
            {
                itemsDictionary[keys[i]] = values[i];
            }

            return itemsDictionary;
        }
    }
}