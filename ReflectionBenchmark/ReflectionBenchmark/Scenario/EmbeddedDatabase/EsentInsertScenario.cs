using System;
using Microsoft.Isam.Esent.Interop;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// Esent inserts test.
    /// </summary>
    public sealed class EsentInsertScenario : EsentScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => $"ESENT {DatabaseTestConsts.InsertCount} Inserts";

        /// <summary>
        /// Do benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
        {
            var random = new Random();
            for (var j = 0; j < DatabaseTestConsts.InsertCount / 1000; j++)
            {
                using (var transaction = new Transaction(sesid))
                {
                    try
                    {
                        for (var i = 0; i < 1000; i++)
                        {
                            Api.JetPrepareUpdate(sesid, tableid, JET_prep.Insert);
                            var dataRow = new DataRow()
                            {
                                DoubleValue = random.NextDouble(),
                                StringValue = i.ToString()
                            };
                            EsentHelpers.UpdateRow(dataRow, sesid, tableid, ref mappings);
                            Api.JetUpdate(sesid, tableid);
                        }
                    }
                    finally
                    {
                        transaction.Commit(CommitTransactionGrbit.LazyFlush);
                    }
                }
            }
            return DatabaseTestConsts.InsertCount;
        }
    }

    /// <summary>
    /// Esent inserts test.
    /// </summary>
    public sealed class EsentInsertScenarioWithTransaction : EsentScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => $"ESENT {DatabaseTestConsts.InsertCount} Inserts (individual transactions)";

        /// <summary>
        /// Do benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
        {
            var random = new Random();
            for (var i = 0; i < DatabaseTestConsts.InsertCount; i++)
            {
                using (var transaction = new Transaction(sesid))
                {
                    try
                    {
                        Api.JetPrepareUpdate(sesid, tableid, JET_prep.Insert);
                        var dataRow = new DataRow()
                        {
                            DoubleValue = random.NextDouble(),
                            StringValue = i.ToString()
                        };
                        EsentHelpers.UpdateRow(dataRow, sesid, tableid, ref mappings);
                        Api.JetUpdate(sesid, tableid);
                    }
                    finally
                    {
                        transaction.Commit(CommitTransactionGrbit.LazyFlush);
                    }
                }
            }
            return DatabaseTestConsts.InsertCount;
        }
    }
}