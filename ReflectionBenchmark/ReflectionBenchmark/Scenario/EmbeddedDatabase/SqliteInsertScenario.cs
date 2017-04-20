using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// SQLITE insert scenario.
    /// </summary>
    public sealed class SqliteInsertScenario : SqliteScenarioBase
    {
        public override string ScenarioName => $"SQLite {DatabaseTestConsts.InsertCount} Inserts";

        /// <summary>
        /// Benchmark.
        /// </summary>
        /// <param name="db">Databse connection.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(SqliteConnection db)
        {
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
            return DatabaseTestConsts.InsertCount;
        }
    }
}