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
        public override string ScenarioName => $"SQLite {DatabaseTestConsts.QueryCount} Queries";

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="db">Database connection.</param>
        protected override void PrepareBenchmark(SqliteConnection db)
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
        }

        /// <summary>
        /// Benchmark.
        /// </summary>
        /// <param name="db">Databse connection.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(SqliteConnection db)
        {
            using (var transaction = db.BeginTransaction())
            {
                var random = new Random();
                for (var i = 0; i < DatabaseTestConsts.QueryCount; i++)
                {
                    double sum = 0.0;
                    var command = new SqliteCommand($"SELECT * FROM test WHERE {nameof(DataRow.DoubleValue)} > @Comparand", db, transaction);
                    command.Parameters.AddWithValue("@Comparand", random.NextDouble());
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var row = SqliteHelpers.LoadRow(reader);
                        sum += row.DoubleValue;
                    }
                }
                return DatabaseTestConsts.QueryCount;
            }
        }
    }
}