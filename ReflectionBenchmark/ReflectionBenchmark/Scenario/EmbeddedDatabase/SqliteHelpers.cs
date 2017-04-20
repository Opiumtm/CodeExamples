using System;
using Microsoft.Data.Sqlite;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// SQLite helpers.
    /// </summary>
    public static class SqliteHelpers
    {
        public static void Insert(SqliteConnection db, DataRow row, string table)
        {
            var command = new SqliteCommand($"INSERT INTO {table} VALUES (NULL, @String, @Double)", db);
            command.Parameters.AddWithValue("@String", row.StringValue);
            command.Parameters.AddWithValue("@Double", row.DoubleValue);
            command.ExecuteNonQuery();
        }

        public static void Insert(SqliteConnection db, SqliteTransaction transaction, DataRow row, string table)
        {
            var command = new SqliteCommand($"INSERT INTO {table} VALUES (NULL, @String, @Double)", db, transaction);
            command.Parameters.AddWithValue("@String", row.StringValue);
            command.Parameters.AddWithValue("@Double", row.DoubleValue);
            command.ExecuteNonQuery();
        }

        public static void InitializeTable(SqliteConnection db, string table)
        {
            var command = new SqliteCommand($"CREATE TABLE IF NOT EXISTS {table} ({nameof(DataRow.Id)} INTEGER PRIMARY KEY AUTOINCREMENT, {nameof(DataRow.StringValue)} TEXT NULL, {nameof(DataRow.DoubleValue)} REAL NOT NULL)", db);
            command.ExecuteNonQuery();
            var indexCommand1 = new SqliteCommand($"CREATE INDEX IF NOT EXISTS IX_{table}_{nameof(DataRow.StringValue)} ON {table} ({nameof(DataRow.StringValue)})", db);
            indexCommand1.ExecuteNonQuery();
            var indexCommand2 = new SqliteCommand($"CREATE INDEX IF NOT EXISTS IX_{table}_{nameof(DataRow.DoubleValue)} ON {table} ({nameof(DataRow.DoubleValue)})", db);
            indexCommand2.ExecuteNonQuery();
        }

        public static DataRow LoadRow(SqliteDataReader reader)
        {
            object[] value = new object[3];
            reader.GetValues(value);
            long id = (Int64) value[0];
            return new DataRow()
            {
                Id = unchecked ((int)id),
                StringValue = (string)value[1],
                DoubleValue = (double)value[2],
            };
        }
    }
}