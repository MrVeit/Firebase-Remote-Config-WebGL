using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FRCWebGL.Data
{
    [Serializable]
    public sealed class SerializableDictionary
    {
        [JsonProperty("keys")]
        public string[] keys;

        [JsonProperty("values")]
        public string[] values;

        public static SerializableDictionary From(
            Dictionary<string, object> dictionary)
        {
            var fakeDictionary = new SerializableDictionary
            {
                keys = dictionary.Keys.ToArray(),
                values = dictionary.Values.Select(
                    value => value.ToString()).ToArray()
            };

            return fakeDictionary;
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