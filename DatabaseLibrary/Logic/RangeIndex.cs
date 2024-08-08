using DatabaseLibrary.Models;

namespace DatabaseLibrary.Logic
{
    /// <summary>
    /// Represents an index that allows range queries based on a property's value.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the index.</typeparam>
    internal class RangeIndex<T> : Index<T> where T : class
    {
        /// <summary>
        /// Dictionary to store sorted property values and their associated records.
        /// </summary>
        private SortedDictionary<object, LinkedList<T>> _rangeIndex;


        /// <summary>
        /// Initializes a new instance of the RangeIndex class with the specified property name.
        /// </summary>
        /// <param name="propertyName">The name of the property to index.</param>
        internal RangeIndex(string propertyName)
        {
            _rangeIndex = new SortedDictionary<object, LinkedList<T>>(); // Initialize the sorted dictionary.
            PropertyName = propertyName; // Set the property name.
            _property = typeof(T).GetProperty(PropertyName); // Get the PropertyInfo object for the specified property.
            if (_property == null || !_property.CanRead)
                throw new InvalidOperationException($"Property {PropertyName} is not valid or not readable.");
        }

        /// <summary>
        /// Adds a record to the range index.
        /// </summary>
        /// <param name="item">The record to add to the index.</param>
        internal override void AddRecord(T item)
        {
            var key = _property.GetValue(item);// Get the value of the property for the specified item.
            if (key == null)
                throw new NullReferenceException($"Property '{PropertyName}' is null");
            if (_rangeIndex.ContainsKey(key))
                _rangeIndex[key].AddLast(item);// Add the item to the existing list of records associated with the key.
            else
            {
                var list = new LinkedList<T>();// Create a new list for the key if it doesn't exist.
                list.AddLast(item);
                _rangeIndex.Add(key, list);
            }
        }

        /// <summary>
        /// Removes a record from the range index.
        /// </summary>
        /// <param name="item">The record to remove from the index.</param>
        internal override void RemoveRecord(T item)
        {
            var key = _property.GetValue(item);// Get the value of the property for the specified item.
            if (key == null)
                throw new NullReferenceException($"Property '{PropertyName}' is null");
            if (_rangeIndex.ContainsKey(key))
            {
                if (_rangeIndex[key].Count == 1)
                    _rangeIndex.Remove(key);// Remove the key from the dictionary if it only has one associated record.
                else
                {
                    foreach (var value in _rangeIndex[key])
                    {
                        if (item.Equals(value))
                        {
                            _rangeIndex[key].Remove(value);// Remove the specified record from the list of records associated with the key.
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tries to retrieve the list of records associated with the specified range of property values from the range index.
        /// </summary>
        /// <param name="propMinValue">The minimum value of the property range.</param>
        /// <param name="propMaxValue">The maximum value of the property range.</param>
        /// <param name="values">When this method returns, contains the list of records within the specified range, if any; otherwise, an empty list.</param>
        /// <returns>true if records were found within the specified range; otherwise, false.</returns>
        internal bool TryGetValue(object propMinValue, object propMaxValue, out LinkedList<T> values)
        {
            values = new LinkedList<T>();// Initialize the list of values.
            bool isInRange = false;
            foreach (var keyValuePair in _rangeIndex)
            {
                if (!isInRange && Comparer<object>.Default.Compare(keyValuePair.Key, propMinValue) >= 0)
                    isInRange = true;// Set isInRange to true when the minimum value is encountered.
                if (isInRange)
                {
                    foreach (var value in keyValuePair.Value)
                    {
                        values.AddLast(value);// Add the records within the range to the values list.
                    }
                }
                if (isInRange && Comparer<object>.Default.Compare(keyValuePair.Key, propMaxValue) >= 0)
                    break;// Break the loop when the maximum value is encountered.
            }
            return values.Count > 0; // Return true if records were found within the specified range; otherwise, false.
        }
    }
}
