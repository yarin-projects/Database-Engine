# Database Engine Example

## Overview

This repository demonstrates a basic implementation of a database engine with various types of indices. The database engine supports unique indices, non-unique indices, and range indices. The provided code defines a set of classes to manage these indices and the database records.

The project was completed on April, 2024.

## Code Structure

### Index<T>

An abstract class representing an index for database records.

- `PropertyName`: The name of the property used for indexing.
- `AddRecord(T item)`: Abstract method to add a record to the index.
- `RemoveRecord(T item)`: Abstract method to remove a record from the index.

### Database<T>

A class that represents a simple in-memory database.

- `Count`: The number of records in the database.
- `AddRecord(T item)`: Adds a record to the database.
- `RemoveRecord(T item)`: Removes a record from the database.
- `GetEnumerator()`: Returns an enumerator for iterating over the records.

### UniqueIndex<T>

A class for unique indices, ensuring that property values are unique.

- `AddRecord(T item)`: Adds a record to the unique index.
- `RemoveRecord(T item)`: Removes a record from the unique index.
- `TryGetValue(object key, out T value)`: Retrieves a record by key.

### RangeIndex<T>

A class for range indices, allowing retrieval of records within a specified range.

- `AddRecord(T item)`: Adds a record to the range index.
- `RemoveRecord(T item)`: Removes a record from the range index.
- `TryGetValue(object propMinValue, object propMaxValue, out LinkedList<T> values)`: Retrieves records within a range.

### NonUniqueIndex<T>

A class for non-unique indices, supporting multiple records with the same property value.

- `AddRecord(T item)`: Adds a record to the non-unique index.
- `RemoveRecord(T item)`: Removes a record from the non-unique index.
- `TryGetValue(object key, out LinkedList<T> value)`: Retrieves records by key.

### DatabaseEngine<T>

A class that manages the database and its indices.

- `AddRecord(T item)`: Adds a record to the database and all indices.
- `RemoveRecord(T item)`: Removes a record from the database and all indices.
- `CreateUniqueIndex(string propertyName)`: Creates a unique index.
- `DeleteUniqueIndex(string propertyName)`: Deletes a unique index.
- `CreateIndex(string propertyName)`: Creates a non-unique index.
- `DeleteIndex(string propertyName)`: Deletes a non-unique index.
- `CreateRangeIndex(string propertyName)`: Creates a range index.
- `DeleteRangeIndex(string propertyName)`: Deletes a range index.
- `SelectValueFromUniqueIndex(string propName, object propValue)`: Retrieves a value from a unique index.
- `SelectValueFromIndex(string propName, object propValue)`: Retrieves values from a non-unique index.
- `SelectValueFromRangeIndex(string propName, object propMinValue, object propMaxValue)`: Retrieves values from a range index.
- `UpdateRecordByItem(T oldRecord, T newRecord)`: Updates a record by replacing it with a new record.
- `UpdateRecordByKey(string propName, object propValue, T updatedRecord)`: Updates a record by key.
- `BulkUpdatePropByKey(string propName, object propValue, object newPropValue)`: Bulk updates a property value.
