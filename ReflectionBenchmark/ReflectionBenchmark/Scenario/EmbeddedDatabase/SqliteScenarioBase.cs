using System;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Data.Sqlite;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// SQLite scenario base
    /// </summary>
    public abstract class SqliteScenarioBase : IScenario
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public abstract string ScenarioName { get; }

        /// <summary>
        /// Do benchmarking (if test is synchronous, run it on background thread).
        /// </summary>
        /// <returns>Benchmark result.</returns>
        public async Task<BenchmarkResult> DoBenchmark()
        {
            var path = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync("sqlite", CreationCollisionOption.OpenIfExists);
            var dbName = path.Path + "\\test.db";
            try
            {
                var file = await path.GetFileAsync("test.db");
                await file.DeleteAsync();
            }
            catch
            {
                // ignore
            }
            return await Task.Factory.StartNew(() =>
            {
                using (var db = new SqliteConnection($"Filename={dbName}"))
                {
                    db.Open();
                    try
                    {
                        SqliteHelpers.InitializeTable(db, "test");
                        PrepareBenchmark(db);
                        var ticks1 = Environment.TickCount;
                        var iterations = DoBenchmark(db);
                        var ticks2 = Environment.TickCount;
                        return new BenchmarkResult() { RunCount = iterations, Milliseconds = ticks2 - ticks1 };
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            });
        }

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="db">Database connection.</param>
        protected virtual void PrepareBenchmark(SqliteConnection db)
        {
        }

        /// <summary>
        /// Benchmark.
        /// </summary>
        /// <param name="db">Databse connection.</param>
        /// <returns>Iteration count.</returns>
        protected abstract int DoBenchmark(SqliteConnection db);
    }
}