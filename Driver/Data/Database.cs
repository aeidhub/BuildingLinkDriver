using System.Data.SQLite;

namespace BuildingLink_Driver.Data
{
    public class Database
    {
        private readonly SQLiteConnection _connection;

        public Database(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public void CreateTableAndDatabaseIfNotExist()
        {
            // Create the database, table, and seed user, if database doesn't exist
            CreateDriversTable();
            AddSeedUser();
        }

        private void CreateDriversTable()
        {
            using SQLiteConnection connection = new(_connection);
            connection.Open();

            using SQLiteCommand command = new("CREATE TABLE IF NOT EXISTS drivers (id INTEGER PRIMARY KEY AUTOINCREMENT, firstName TEXT NOT NULL, lastName TEXT NOT NULL, email TEXT NOT NULL UNIQUE, phoneNumber TEXT NOT NULL UNIQUE);", connection);
            command.ExecuteNonQuery();
        }

        private void AddSeedUser()
        {
            using SQLiteConnection connection = new(_connection);
            connection.Open();

            using SQLiteCommand command = new("INSERT OR IGNORE INTO drivers (firstName, lastName, email, phoneNumber) VALUES ('John', 'Doe', 'john.doe@example.com', '(123) 456-7890')", connection);
            command.ExecuteNonQuery();
        }
    }
}
