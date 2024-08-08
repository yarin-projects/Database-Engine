using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using DatabaseLibrary.Models;

namespace DatabaseLibrary.Logic
{
    /// <summary>
    /// Represents an index that enforces uniqueness of values for a specific property.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the index.</typeparam>
    internal class UniqueIndex<T> : Index<T> where T : class
    {
        /// <summary>
        /// Dictionary to store unique property values and their associated records.
        /// </summary>
        private Dictionary<object, T> _uniqueIndex;

        /// <summary>
        /// Initializes a new instance of the UniqueIndex class with the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property to index.</param>
        internal UniqueIndex(string propertyName)
        {
            _uniqueIndex = new Dictionary<object, T>(); // Initialize the dictionary.
            PropertyName = propertyName; // Set the property name.
            _property = typeof(T).GetProperty(PropertyName); // Get the PropertyInfo object for the specified property.
            if (_property == null || !_property.CanRead)
                throw new InvalidOperationException($"Property {PropertyName} is not valid or not readable.");
        }

        /// <summary>
        /// Adds a record to the unique index.
        /// </summary>
        /// <param name="item">The record to add to the index.</param>
        internal override void AddRecord(T item)
        {
            var key = _property.GetValue(item); // Get the value of the property for the specified item.
            if (key == null)
                throw new NullReferenceException($"Property '{PropertyName}' is null");
            if (!_uniqueIndex.TryAdd(key, item))
                throw new ArgumentException($"Property '{PropertyName}' isn't unique.\n" +
                                            $"Duplicate values in the property '{PropertyName}' exist");// Throw an exception if the property value is not unique.
        }

        /// <summary>
        /// Removes a record from the unique index.
        /// </summary>
        /// <param name="item">The record to remove from the index.</param>
        internal override void RemoveRecord(T item)
        {
            var key = _property.GetValue(item);// Get the value of the property for the specified item.
            if (key == null)
                throw new NullReferenceException($"Property '{PropertyName}' is null");
            if (_uniqueIndex.ContainsKey(key))
                _uniqueIndex.Remove(key);// Remove the key from the dictionary.
        }

        /// <summary>
        /// Tries to retrieve the record associated with the specified key from the unique index.
        /// </summary>
        /// <param name="key">The key to search for in the index.</param>
        /// <param name="value">When this method returns, contains the record associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the index contains an element with the specified key; otherwise, false.</returns>
        internal bool TryGetValue(object key, out T value)
        {
            return _uniqueIndex.TryGetValue(key, out value); // Try to retrieve the value associated with the specified key from the dictionary.
        }
    }
}
