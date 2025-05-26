using System;
using System.Collections.Generic;

namespace FRCWebGL.Data
{
    [Serializable]
    public sealed class SerializableDictionary
    {
        public Dictionary<string, string> dictionary;

        public Dictionary<string, string> ToDictionary() => dictionary;
    }
}