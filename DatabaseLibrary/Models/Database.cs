using System.Collections;

namespace DatabaseLibrary.Models
{
    /// <summary>
    /// Represents a generic database that can store objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of objects to be stored in the database.</typeparam>
    internal class Database<T> : IEnumerable<T> where T : class
    {
        /// <summary>
        /// Internal storage for the database.
        /// </summary>
        private LinkedList<T> _table;

        /// <summary>
        /// Gets the number of records in the database.
        /// </summary>
        internal int Count { get => _table.Count; }

        /// <summary>
        /// Initializes a new instance of the Database class.
        /// </summary>
        internal Database()
        {
            _table = new LinkedList<T>();
        }

        /// <summary>
        /// Adds a record to the database.
        /// </summary>
        /// <param name="item">The record to be added.</param>
        internal void AddRecord(T item)
        {
            _table.AddLast(item);
        }

        /// <summary>
        /// Removes a record from the database.
        /// </summary>
        /// <param name="item">The record to be removed.</param>
        internal void RemoveRecord(T item)
        {
            _table.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _table)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
