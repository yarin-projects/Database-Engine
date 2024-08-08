using DatabaseLibrary.Exceptions;
using DatabaseLibrary.Logic;
using DatabaseLibrary.Models;

namespace DatabaseLibrary.API
{
    /// <summary>
    /// Represents an engine for managing a database with various types of indices.
    /// </summary>
    /// <typeparam name="T">The type of objects stored in the database.</typeparam>
    public class DatabaseEngine<T> where T : class
    {
        private Database<T> _table; // Database to store records.
        private LinkedList<UniqueIndex<T>> _uniqueIndices; // List of unique indices.
        private LinkedList<NonUniqueIndex<T>> _nonUniqueIndices; // List of non-unique indices.
        private LinkedList<RangeIndex<T>> _rangeIndices; // List of range indices.

        /// <summary>
        /// Initializes a new instance of the DatabaseEngine class.
        /// </summary>
        public DatabaseEngine()
        {
            _table = new Database<T>(); // Initialize the database.
            _uniqueIndices = new LinkedList<UniqueIndex<T>>(); // Initialize the list of unique indices.
            _nonUniqueIndices = new LinkedList<NonUniqueIndex<T>>(); // Initialize the list of non-unique indices.
            _rangeIndices = new LinkedList<RangeIndex<T>>(); // Initialize the list of range indices.
        }

        /// <summary>
        /// Adds a record to the database and all associated indices.
        /// </summary>
        /// <param name="item">The record to add.</param>
        public void AddRecord(T item)
        {
            _table.AddRecord(item); // Add the record to the database.
            AddRecordToIndices(item); // Add the record to all indices.
        }

        /// <summary>
        /// Removes a record from the database and all associated indices.
        /// </summary>
        /// <param name="item">The record to remove.</param>
        public void RemoveRecord(T item)
        {
            _table.RemoveRecord(item); // Remove the record from the database.
            RemoveRecordFromIndices(item); // Remove the record from all indices.
        }

        /// <summary>
        /// Creates a unique index for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to create the index for.</param>
        public void CreateUniqueIndex(string propertyName)
        {
            DatabaseEngine<T>.ThrowIfIndexExists(_uniqueIndices, propertyName); // Check if an index with the same property name already exists.
            var uniqueIndex = new UniqueIndex<T>(propertyName); // Create a new unique index.
            AddExistingRecordsToIndex(uniqueIndex); // Add existing records to the index.
            _uniqueIndices.AddLast(uniqueIndex); // Add the index to the list of unique indices.
        }

        /// <summary>
        /// Deletes a unique index for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property associated with the index to delete.</param>
        public void DeleteUniqueIndex(string propertyName)
        {
            DatabaseEngine<T>.RemoveIndexFromDB(_uniqueIndices, propertyName); // Remove the unique index from the list of indices.
        }

        /// <summary>
        /// Creates a non-unique index for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to create the index for.</param>
        public void CreateIndex(string propertyName)
        {
            DatabaseEngine<T>.ThrowIfIndexExists(_nonUniqueIndices, propertyName); // Check if an index with the same property name already exists.
            var index = new NonUniqueIndex<T>(propertyName); // Create a new non-unique index.
            AddExistingRecordsToIndex(index); // Add existing records to the index.
            _nonUniqueIndices.AddLast(index); // Add the index to the list of non-unique indices.
        }

        /// <summary>
        /// Deletes a non-unique index for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property associated with the index to delete.</param>
        public void DeleteIndex(string propertyName)
        {
            DatabaseEngine<T>.RemoveIndexFromDB(_nonUniqueIndices, propertyName); // Remove the non-unique index from the list of indices.
        }

        /// <summary>
        /// Creates a range index for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property to create the index for.</param>
        public void CreateRangeIndex(string propertyName)
        {
            DatabaseEngine<T>.ThrowIfIndexExists(_rangeIndices, propertyName); // Check if an index with the same property name already exists.
            var rangeIndex = new RangeIndex<T>(propertyName); // Create a new range index.
            AddExistingRecordsToIndex(rangeIndex); // Add existing records to the index.
            _rangeIndices.AddLast(rangeIndex); // Add the index to the list of range indices.
        }

        /// <summary>
        /// Deletes a range index for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property associated with the index to delete.</param>
        public void DeleteRangeIndex(string propertyName)
        {
            DatabaseEngine<T>.RemoveIndexFromDB(_rangeIndices, propertyName); // Remove the range index from the list of indices.
        }

        /// <summary>
        /// Retrieves a value from the unique index based on the specified property name and value.
        /// </summary>
        public T SelectValueFromUniqueIndex(string propName, object propValue)
        {
            if (_uniqueIndices.Count < 1)
                throw new NoUniqueIndicesAddedException("Engine has no unique indices.");
            foreach (var uniqueIndex in _uniqueIndices)
            {
                if (uniqueIndex.PropertyName == propName)
                {
                    if (!uniqueIndex.TryGetValue(propValue, out T value))
                        throw new ItemNotFoundException($"Item with the key '{propValue}' doesn't exist");
                    return value;
                }
            }
            throw new PropertyNotFoundException($"Engine has no Unique Index with the property {propName}");
        }

        /// <summary>
        /// Retrieves values from the non-unique index based on the specified property name and value.
        /// </summary>
        public IEnumerable<T> SelectValueFromIndex(string propName, object propValue)
        {
            if (_nonUniqueIndices.Count < 1)
                throw new NoIndicesAddedException("Engine has no indices.");
            foreach (var index in _nonUniqueIndices)
            {
                if (index.PropertyName == propName)
                {
                    if (!index.TryGetValue(propValue, out LinkedList<T> value))
                        throw new ItemNotFoundException($"Item with the key '{propValue}' doesn't exist");
                    return value;
                }
            }
            throw new PropertyNotFoundException($"Engine has no Index with the property {propName}");
        }

        /// <summary>
        /// Retrieves values from the range index based on the specified property name, minimum value, and maximum value.
        /// </summary>
        public IEnumerable<T> SelectValueFromRangeIndex(string propName, object propMinValue, object propMaxValue)
        {
            if (_rangeIndices.Count < 1)
                throw new NoRangeIndicesAddedException("Engine has no range indices.");
            foreach (var _rangeIndex in _rangeIndices)
            {
                if (_rangeIndex.PropertyName == propName)
                {
                    if (!_rangeIndex.TryGetValue(propMinValue, propMaxValue, out LinkedList<T> values))
                        throw new ItemNotFoundException($"No items within the range {propMinValue}-{propMaxValue} were found.");
                    return values;
                }
            }
            throw new PropertyNotFoundException($"Engine has no Range Index with the property {propName}");
        }

        /// <summary>
        /// Updates a record in the database by replacing it with a new record.
        /// </summary>
        public void UpdateRecordByItem(T oldRecord, T newRecord)
        {
            if (_table.Contains(oldRecord))
            {
                RemoveRecord(oldRecord);
                AddRecord(newRecord);
            }
            else
                throw new ItemNotFoundException("Old record was not found in the database");
        }

        /// <summary>
        /// Updates a record in the database by replacing the old record with the updated record,
        /// based on the specified property name and value.
        /// </summary>
        public void UpdateRecordByKey(string propName, object propValue, T updatedRecord)
        {
            T oldRecord = SelectValueFromUniqueIndex(propName, propValue);
            RemoveRecord(oldRecord);
            AddRecord(updatedRecord);
        }

        /// <summary>
        /// Bulk updates a property value of records in the database based on the specified property name and value.
        /// </summary>
        public void BulkUpdatePropByKey(string propName, object propValue, object newPropValue)
        {
            T[] records = SelectValueFromIndex(propName, propValue).ToArray();
            var prop = typeof(T).GetProperty(propName);
            foreach (var record in records)
            {
                RemoveRecord(record);
                prop.SetValue(record, newPropValue);
                AddRecord(record);
            }
        }

        /// <summary>
        /// Removes an index from the database engine.
        /// </summary>
        /// <typeparam name="TIndex">The type of index to remove.</typeparam>
        /// <param name="indices">The list of indices to search for the index.</param>
        /// <param name="propertyName">The name of the property associated with the index.</param>
        private static void RemoveIndexFromDB<TIndex>(LinkedList<TIndex> indices, string propertyName) where TIndex : Index<T>
        {
            foreach (var index in indices)
            {
                if (index.PropertyName == propertyName)
                {
                    indices.Remove(index); // Remove the index from the list.
                    return;
                }
            }
            // If the index is not found, throw an exception.
            string msg = indices.GetType() == typeof(LinkedList<NonUniqueIndex<T>>) ?
                $"Engine has no Index with the property {propertyName}" :
                indices.GetType() == typeof(LinkedList<UniqueIndex<T>>) ?
                $"Engine has no Unique Index with the property {propertyName}" :
                $"Engine has no Range Index with the property {propertyName}";
            throw new PropertyNotFoundException(msg);
        }

        /// <summary>
        /// Adds existing records to the specified index.
        /// </summary>
        /// <typeparam name="TIndex">The type of index.</typeparam>
        /// <param name="index">The index to which records will be added.</param>
        private void AddExistingRecordsToIndex<TIndex>(TIndex index) where TIndex : Index<T>
        {
            foreach (var item in _table)
            {
                index.AddRecord(item); // Add each record to the index.
            }
        }

        /// <summary>
        /// Throws an exception if an index with the specified property name already exists.
        /// </summary>
        /// <typeparam name="TIndex">The type of index.</typeparam>
        /// <param name="indices">The list of indices to check for duplicates.</param>
        /// <param name="propertyName">The name of the property associated with the index.</param>
        private static void ThrowIfIndexExists<TIndex>(LinkedList<TIndex> indices, string propertyName) where TIndex : Index<T>
        {
            foreach (var index in indices)
            {
                if (index.PropertyName == propertyName)
                {
                    // If an index with the same property name exists, throw an exception.
                    string msg = indices.GetType() == typeof(LinkedList<NonUniqueIndex<T>>) ?
                        $"Property '{propertyName}' already has an index" :
                        indices.GetType() == typeof(LinkedList<UniqueIndex<T>>) ?
                        $"Property '{propertyName}' already has a unique index" :
                        $"Property '{propertyName}' already has a range index";
                    throw new InvalidOperationException(msg);
                }
            }
        }


        /// <summary>
        /// Adds a record to all indices in the database engine.
        /// </summary>
        /// <param name="item">The record to add to the indices.</param>
        private void AddRecordToIndices(T item)
        {
            foreach (var uniqueIndex in _uniqueIndices)
            {
                uniqueIndex.AddRecord(item); // Add the record to each unique index.
            }
            foreach (var index in _nonUniqueIndices)
            {
                index.AddRecord(item); // Add the record to each non-unique index.
            }
            foreach (var rangeIndex in _rangeIndices)
            {
                rangeIndex.AddRecord(item); // Add the record to each range index.
            }
        }

        /// <summary>
        /// Removes a record from all indices in the database engine.
        /// </summary>
        /// <param name="item">The record to remove from the indices.</param>
        private void RemoveRecordFromIndices(T item)
        {
            foreach (var uniqueIndex in _uniqueIndices)
            {
                uniqueIndex.RemoveRecord(item); // Remove the record from each unique index.
            }
            foreach (var index in _nonUniqueIndices)
            {
                index.RemoveRecord(item); // Remove the record from each non-unique index.
            }
            foreach (var rangeIndex in _rangeIndices)
            {
                rangeIndex.RemoveRecord(item); // Remove the record from each range index.
            }
        }
    }
}
