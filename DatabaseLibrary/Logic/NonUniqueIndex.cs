using DatabaseLibrary.Models;

namespace DatabaseLibrary.Logic
{
    /// <summary>
    /// Represents an index that allows non-unique values for a specific property.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the index.</typeparam>
    internal class NonUniqueIndex<T> : Index<T> where T : class
    {
        /// <summary>
        /// Dictionary to store non-unique values and their associated records.
        /// </summary>
        private Dictionary<object, LinkedList<T>> _nonUniqueIndex;

        /// <summary>
        /// Initializes a new instance of the NonUniqueIndex class with the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property to index.</param>
        internal NonUniqueIndex(string propertyName)
        {
            _nonUniqueIndex = new Dictionary<object, LinkedList<T>>(); // Initialize the dictionary.
            PropertyName = propertyName; // Set the property name.
            _property = typeof(T).GetProperty(PropertyName); // Get the PropertyInfo object for the specified property.
            if (_property == null || !_property.CanRead)
                throw new InvalidOperationException($"Property {PropertyName} is not valid or not readable.");
        }

        /// <summary>
        /// Adds a record to the non-unique index.
        /// </summary>
        /// <param name="item">The record to add to the index.</param>
        internal override void AddRecord(T item)
        {
            var key = _property.GetValue(item) ?? throw new NullReferenceException($"Property '{PropertyName}' is null");// Get the value of the property for the specified item.
            if (_nonUniqueIndex.TryGetValue(key, out LinkedList<T>? value))
                value.AddLast(item);// Add the item to the existing list of records associated with the key.
            else
            {
                var list = new LinkedList<T>();// Create a new list for the key if it doesn't exist.
                list.AddLast(item);
                _nonUniqueIndex.Add(key, list);
            }
        }

        /// <summary>
        /// Removes a record from the non-unique index.
        /// </summary>
        /// <param name="item">The record to remove from the index.</param>
        internal override void RemoveRecord(T item)
        {
            var key = _property.GetValue(item);// Get the value of the property for the specified item.
            if (key == null)
                throw new NullReferenceException($"Property '{PropertyName}' is null");
            if (_nonUniqueIndex.ContainsKey(key))
            {
                if (_nonUniqueIndex[key].Count == 1)
                    _nonUniqueIndex.Remove(key);// Remove the key from the dictionary if it only has one associated record.
                else
                {
                    foreach (var value in _nonUniqueIndex[key])
                    {
                        if (item.Equals(value))
                        {
                            _nonUniqueIndex[key].Remove(value);// Remove the specified record from the list of records associated with the key.
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tries to retrieve the list of records associated with the specified key from the non-unique index.
        /// </summary>
        /// <param name="key">The key to search for in the index.</param>
        /// <param name="value">When this method returns, contains the list of records associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns>true if the index contains an element with the specified key; otherwise, false.</returns>
        internal bool TryGetValue(object key, out LinkedList<T> value)
        {
            return _nonUniqueIndex.TryGetValue(key, out value);
        }
    }
}
