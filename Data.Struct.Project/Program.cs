using DatabaseLibrary.API;
using DataStructProjYarin.Models;

namespace DataStructProjYarin
{
    internal class Program
    {
        static void Main()
        {

            DatabaseEngine<Customer> databaseEngine = AddCustomersToEngine(out Customer customer);

            CreateIndices(databaseEngine);

            DeleteIndicesAndTryDeleteNullIndex(databaseEngine);

            CreateExistingIndex(databaseEngine);

            SelectValuesFromIndices(databaseEngine);

            RemoveRecordsFromDB(databaseEngine);

            databaseEngine.UpdateRecordByKey("Id", 3, new Customer() { Id = 55, Age = 20, Name = "abc" });

            databaseEngine.UpdateRecordByItem(customer, new Customer() { Id = 42, Age = 22, Name = "cba" });

            databaseEngine.BulkUpdatePropByKey("Age", 30, 333);
        }

        private static void RemoveRecordsFromDB(DatabaseEngine<Customer> databaseEngine)
        {
            Customer customer21 = new() { Id = 21, Age = 22, Name = "Uma" };
            Customer customer22 = new() { Id = 22, Age = 32, Name = "Victor" };
            databaseEngine.AddRecord(customer21);
            databaseEngine.AddRecord(customer22);
            databaseEngine.RemoveRecord(customer21);
            databaseEngine.RemoveRecord(customer22);
        }

        private static void SelectValuesFromIndices(DatabaseEngine<Customer> databaseEngine)
        {
            Customer c21 = databaseEngine.SelectValueFromUniqueIndex("Id", 3);
            Customer[] customers1 = databaseEngine.SelectValueFromIndex("Age", 30).ToArray();
            Customer[] customers2 = databaseEngine.SelectValueFromRangeIndex("Id", -2, 7).ToArray();
        }

        private static void CreateExistingIndex(DatabaseEngine<Customer> databaseEngine)
        {
            databaseEngine.CreateUniqueIndex("Id");
            databaseEngine.CreateRangeIndex("Id");
            databaseEngine.CreateIndex("Id");
            try
            {
                databaseEngine.CreateUniqueIndex("Id");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void DeleteIndicesAndTryDeleteNullIndex(DatabaseEngine<Customer> databaseEngine)
        {
            databaseEngine.DeleteUniqueIndex("Id");
            databaseEngine.DeleteIndex("Id");
            databaseEngine.DeleteRangeIndex("Id");
            try
            {
                databaseEngine.DeleteIndex("aaa");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateIndices(DatabaseEngine<Customer> databaseEngine)
        {
            databaseEngine.CreateUniqueIndex("Id");
            try
            {
                databaseEngine.CreateUniqueIndex("Age");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                databaseEngine.CreateUniqueIndex("Name");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            databaseEngine.CreateRangeIndex("Id");
            databaseEngine.CreateRangeIndex("Age");
            databaseEngine.CreateRangeIndex("Name");
            databaseEngine.CreateIndex("Id");
            databaseEngine.CreateIndex("Age");
            databaseEngine.CreateIndex("Name");
        }

        private static DatabaseEngine<Customer> AddCustomersToEngine(out Customer customer1)
        {
            customer1 = new Customer() { Id = 1, Age = 10, Name = "Alice" };
            Customer customer2 = new() { Id = 2, Age = 20, Name = "Bob" };
            Customer customer3 = new() { Id = 3, Age = 30, Name = "Charlie" };
            Customer customer4 = new() { Id = 4, Age = 30, Name = "David" };
            Customer customer5 = new() { Id = 5, Age = 30, Name = "Emma" };
            Customer customer6 = new() { Id = 6, Age = 30, Name = "Frank" };
            Customer customer7 = new() { Id = 7, Age = 25, Name = "Grace" };
            Customer customer8 = new() { Id = 8, Age = 35, Name = "Henry" };
            Customer customer9 = new() { Id = 9, Age = 45, Name = "a" };
            Customer customer10 = new() { Id = 10, Age = 55, Name = "a" };
            Customer customer11 = new() { Id = 11, Age = 18, Name = "a" };
            Customer customer12 = new() { Id = 12, Age = 28, Name = "a" };
            Customer customer13 = new() { Id = 13, Age = 38, Name = "Mia" };
            Customer customer14 = new() { Id = 14, Age = 48, Name = "Nathan" };
            Customer customer15 = new() { Id = 15, Age = 58, Name = "Olivia" };
            Customer customer16 = new() { Id = 16, Age = 21, Name = "Peter" };
            Customer customer17 = new() { Id = 17, Age = 31, Name = "Queenie" };
            Customer customer18 = new() { Id = 18, Age = 41, Name = "Ryan" };
            Customer customer19 = new() { Id = 19, Age = 51, Name = "Sara" };
            Customer customer20 = new() { Id = 20, Age = 61, Name = "Tom" };
            DatabaseEngine<Customer> databaseEngine = new();
            databaseEngine.AddRecord(customer1);
            databaseEngine.AddRecord(customer2);
            databaseEngine.AddRecord(customer3);
            databaseEngine.AddRecord(customer4);
            databaseEngine.AddRecord(customer5);
            databaseEngine.AddRecord(customer6);
            databaseEngine.AddRecord(customer7);
            databaseEngine.AddRecord(customer8);
            databaseEngine.AddRecord(customer9);
            databaseEngine.AddRecord(customer10);
            databaseEngine.AddRecord(customer11);
            databaseEngine.AddRecord(customer12);
            databaseEngine.AddRecord(customer13);
            databaseEngine.AddRecord(customer14);
            databaseEngine.AddRecord(customer15);
            databaseEngine.AddRecord(customer16);
            databaseEngine.AddRecord(customer17);
            databaseEngine.AddRecord(customer18);
            databaseEngine.AddRecord(customer19);
            databaseEngine.AddRecord(customer20);
            return databaseEngine;
        }
    }
}