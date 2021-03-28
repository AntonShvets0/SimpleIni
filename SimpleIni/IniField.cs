﻿using IniObject.Interfaces;

namespace IniObject
{
    /// <summary>
    /// Part of ini file
    /// </summary>
    public class IniField : IniRoot, IKey
    {
        /// <summary>
        /// Key for field
        /// </summary>
        public string Key { get; set; }
        
        /// <summary>
        /// Value. If is section field, value can be null
        /// </summary>
        public string Value;

        /// <summary>
        /// Create part ini root
        /// </summary>
        /// <param name="key">Key of field</param>
        /// <param name="value">Value of field</param>
        internal IniField(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}