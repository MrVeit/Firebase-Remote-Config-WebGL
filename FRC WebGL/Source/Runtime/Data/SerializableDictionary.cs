using System;
using System.Collections.Generic;

namespace FRCWebGL.Data
{
    [Serializable]
    public sealed class SerializableDictionary
    {
        public string[] keys;
        public string[] values;

        public static SerializableDictionary From(
            Dictionary<string, object> dictionary)
        {
            var dataKeys = new SerializableDictionary
            {
                keys = new string[dictionary.Count],
                values = new string[dictionary.Count]
            };

            int id = 0;

            foreach (var keyValue in dictionary)
            {
                dataKeys.keys[id] = keyValue.Key;
                dataKeys.values[id] = keyValue.Value?.ToString() ?? String.Empty;

                id++;
            }

            return dataKeys;
        }

        public Dictionary<string, string> To()
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