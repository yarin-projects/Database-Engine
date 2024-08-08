using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Models
{
    /// <summary>
    /// Represents an abstract index for storing and accessing records of type T.
    /// </summary>
    /// <typeparam name="T">The type of records stored in the index.</typeparam>
    internal abstract class Index<T> where T : class
    {
        /// <summary>
        ///  The PropertyInfo object representing the indexed property.
        /// </summary>
        protected PropertyInfo? _property; 

        /// <summary>
        /// Gets or sets the name of the property used for indexing.
        /// </summary>
        internal string PropertyName { get; set; }

        /// <summary>
        /// Adds a record to the index.
        /// </summary>
        /// <param name="item">The record to be added to the index.</param>
        internal abstract void AddRecord(T item);

        /// <summary>
        /// Removes a record from the index.
        /// </summary>
        /// <param name="item">The record to be removed from the index.</param>
        internal abstract void RemoveRecord(T item);
    }
}
