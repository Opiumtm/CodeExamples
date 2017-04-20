using System;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Isam.Esent.Interop;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// Scenario base for ESENT tests.
    /// </summary>
    public abstract class EsentScenarioBase : IScenario
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
            var path = await ApplicationData.Current.LocalCacheFolder.CreateFolderAsync("esent", CreationCollisionOption.ReplaceExisting);
            var dbName = path.Path + "\\test.edb";
            return await Task.Factory.StartNew(() =>
            {
                (var instance, var session) = EsentHelpers.OpenSession(path.Path);
                using (instance)
                {
                    using (session)
                    {
                        (var tableid, var dbid, var mappings) = EsentHelpers.InitializeDb(session, dbName);
                        try
                        {
                            PrepareBenchmark(session, dbid, tableid, ref mappings);
                            var ticks1 = Environment.TickCount;
                            var iterations = DoBenchmark(session, dbid, tableid, ref mappings);
                            var ticks2 = Environment.TickCount;
                            return new BenchmarkResult() {RunCount = iterations, Milliseconds = ticks2 - ticks1};
                        }
                        finally
                        {
                            Api.JetCloseTable(session, tableid);
                            Api.JetCloseDatabase(session, dbid, CloseDatabaseGrbit.None);                            
                            Api.JetDetachDatabase(session, dbName);
                            session.End();
                            instance.Term();
                        }
                    }
                }
            });
        }

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        protected virtual void PrepareBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
        {            
        }

        /// <summary>
        /// Do benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        /// <returns>Iteration count.</returns>
        protected abstract int DoBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings);
    }
}