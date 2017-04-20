using System;
using Microsoft.Data.Sqlite;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// SQLITE query scenario.
    /// </summary>
    public sealed class SqliteQueryScenario : SqliteScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => $"SQLite SUM {DatabaseTestConsts.QueryCount} Queries";

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="db">Database connection.</param>
        protected override object PrepareBenchmark(SqliteConnection db)
        {
            base.PrepareBenchmark(db);
            var random = new Random();
            for (var j = 0; j < DatabaseTestConsts.InsertCount / 1000; j++)
            {
                using (var transaction = db.BeginTransaction())
                {
                    for (var i = 0; i < 1000; i++)
                    {
                        var dataRow = new DataRow()
                        {
                            DoubleValue = random.NextDouble(),
                            StringValue = i.ToString()
                        };
                        SqliteHelpers.Insert(db, transaction, dataRow, "test");
                    }
                    transaction.Commit();
                }
            }
            return null;
        }

        /// <summary>
        /// Benchmark.
        /// </summary>
        /// <param name="db">Databse connection.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(SqliteConnection db, object data)
        {
            using (var transaction = db.BeginTransaction())
            {
                var random = new Random();
                double sum = 0.0;
                for (var i = 0; i < DatabaseTestConsts.QueryCount; i++)
                {
                    var command = new SqliteCommand($"SELECT SUM({nameof(DataRow.DoubleValue)}) FROM test WHERE {nameof(DataRow.DoubleValue)} > @Comparand", db, transaction);
                    command.Parameters.AddWithValue("@Comparand", random.NextDouble());
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {                        
                        sum += reader.GetDouble(0);
                    }
                }
                return DatabaseTestConsts.QueryCount;
            }
        }
    }

    /// <summary>
    /// SQLITE query scenario.
    /// </summary>
    public sealed class SqlitePkSeekScenario : SqliteScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => $"SQLite {DatabaseTestConsts.InsertCount} Primary Key Seek";

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="db">Database connection.</param>
        protected override object PrepareBenchmark(SqliteConnection db)
        {
            base.PrepareBenchmark(db);
            var random = new Random();
            for (var j = 0; j < DatabaseTestConsts.InsertCount / 1000; j++)
            {
                using (var transaction = db.BeginTransaction())
                {
                    for (var i = 0; i < 1000; i++)
                    {
                        var dataRow = new DataRow()
                        {
                            DoubleValue = random.NextDouble(),
                            StringValue = i.ToString()
                        };
                        SqliteHelpers.Insert(db, transaction, dataRow, "test");
                    }
                    transaction.Commit();
                }
            }
            var seekArray = new int[DatabaseTestConsts.InsertCount];
            for (var i = 0; i < DatabaseTestConsts.InsertCount; i++)
            {
                seekArray[i] = random.Next(1, DatabaseTestConsts.InsertCount);
            }
            return seekArray;
        }

        /// <summary>
        /// Benchmark.
        /// </summary>
        /// <param name="db">Databse connection.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(SqliteConnection db, object data)
        {
            var seekArray = (int[])data;
            using (var transaction = db.BeginTransaction())
            {
                double sum = 0.0;
                for (var i = 0; i < DatabaseTestConsts.InsertCount; i++)
                {
                    var command = new SqliteCommand($"SELECT * FROM test WHERE {nameof(DataRow.Id)} = @Comparand", db, transaction);
                    command.Parameters.AddWithValue("@Comparand", seekArray[i]);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var row = SqliteHelpers.LoadRow(reader);
                        sum += row.DoubleValue;
                    }
                }
                return DatabaseTestConsts.InsertCount;
            }
        }
    }
}