﻿using IniObject.Interfaces;

namespace IniObject
{
    public class IniSection : IniRoot, IKey
    {
        /// <summary>
        /// Key for section
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Create section
        /// </summary>
        /// <param name="key">Key of section</param>
        internal IniSection(string key)
        {
            Key = key;
        }
    }
}