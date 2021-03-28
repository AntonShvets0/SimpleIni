﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IniObject.Exceptions;
using IniObject.Interfaces;

namespace IniObject
{
    /// <summary>
    /// Main class for Ini File
    /// </summary>
    public class IniRoot : IEnumerable<IniRoot>, IEnumerator<IniRoot>
    {
        /// <summary>
        /// Fields in file
        /// </summary>
        private readonly List<IniRoot> Values = new List<IniRoot>();
        
        public IEnumerator<IniRoot> GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IniRoot Current => Values[_index];
        object IEnumerator.Current => Current;

        private int _index = -1;
        
        public void Dispose() { }
        public void Reset()
        {
            _index = -1;
        }

        public bool MoveNext()
        {
            if (Values.Count - 1 < _index + 1) return false;
            _index++;

            return true;
        }
        
        /// <summary>
        /// It's for Ini class
        /// </summary>
        protected IniRoot() { }
        
        /// <summary>
        /// Parse data from content
        /// </summary>
        /// <param name="content">Ini string</param>
        public IniRoot(string content)
        {
            GetFieldsFromContent(content);
        }

        /// <summary>
        /// Handle content and add to Values variable fields
        /// </summary>
        /// <param name="content">Ini string</param>
        private void GetFieldsFromContent(string content)
        {
            IniSection scope = null;

            var rows = content.Replace("\r", "").Split('\n');
            foreach (var row in rows)
            {
                if (row.StartsWith(";") || string.IsNullOrWhiteSpace(row)) continue;
                if (row.StartsWith("[") && row.EndsWith("]"))
                {
                    scope = GetScope(row);
                    continue;
                }
                
                GetValueFromField(row, scope);
            }
        }

        /// <summary>
        /// Get value field and add to need scope
        /// </summary>
        /// <param name="row">Row</param>
        /// <param name="scope">Section</param>
        /// <exception cref="IniReadException">Throw if wrong syntax</exception>
        private void GetValueFromField(string row, IniSection scope)
        {
            var values = GetValues(scope);
            
            var keyValue = row.Split('=').ToList();
            if (keyValue.Count < 2)
                throw new IniReadException($"Wrong string '{row}' while read ini file");

            var key = keyValue[0].Trim(' ');
            keyValue.RemoveAt(0);
            var value = string.Join('=', keyValue).Trim(' '); 

            var iniField = values.FirstOrDefault(i => i.Key == key);
            if (iniField != null)
            {
                iniField.Value = value;
                return;
            }
            
            values.Add(new IniField(key, value));
        }

        /// <summary>
        /// Get values from section or IniRoot
        /// </summary>
        /// <param name="scope">Null or section</param>
        /// <returns>Values from scope or values IniRoot</returns>
        private List<IniField> GetValues(IniSection scope) => 
            (scope?.Values ?? Values).Select(v => v as IniField).ToList();

        /// <summary>
        /// Get scope from row
        /// </summary>
        /// <param name="row">Row</param>
        /// <returns></returns>
        private IniSection GetScope(string row)
        {
            var scopeName = row.TrimStart('[').TrimEnd(']');

            if (Values.FirstOrDefault(s => 
                s is IniSection section && section.Key == scopeName) is IniSection existsScope) return existsScope;
            
            var newScope = new IniSection(scopeName);
            Values.Add(newScope);
            return newScope;
        }

        /// <summary>
        /// To ini format
        /// </summary>
        /// <returns>Ini string</returns>
        public override string ToString()
        {
            var content = "";

            foreach (var data in Values)
            {
                if (data is IniSection section) content += $"[{section.Key}]\n{section}";
                else if (data is IniField field) content += $"{field.Key} = {field.Value}\n";
            }
            
            return content;
        }
        
        public IniRoot this[string key]
        {
            get => Values.FirstOrDefault(i => 
                i is IKey iniPart && iniPart.Key == key
                );
            set
            {
                var data = Values.FirstOrDefault(i => 
                    i is IKey iniPart && iniPart.Key == key && iniPart.Key == key);
                
                if (data != null)
                    Values.Remove(data);

                if (value is IKey iniKey)
                    iniKey.Key = key;

                Values.Add(value);
            }
        }
        
        /// <summary>
        /// Parse ini file
        /// </summary>
        /// <param name="file">Path to file</param>
        /// <returns>IniRoot</returns>
        public static IniRoot FromFile(string file) => new IniRoot(File.ReadAllText(file));

        /// <summary>
        /// Get value from ini part
        /// </summary>
        /// <param name="ini">Part</param>
        /// <returns>Value of field, or section, or root</returns>
        public static implicit operator string(IniRoot ini)
        {
            if (ini is IniField iniField) return iniField.Value;

            return ini.ToString();
        }

    }
}