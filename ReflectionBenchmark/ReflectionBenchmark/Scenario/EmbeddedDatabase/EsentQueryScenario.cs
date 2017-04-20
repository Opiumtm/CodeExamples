using System;
using Microsoft.Isam.Esent.Interop;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// ESENT Query scenario.
    /// </summary>
    public sealed class EsentQueryScenario : EsentScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => $"ESENT {DatabaseTestConsts.QueryCount} Queries";

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        protected override void PrepareBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
        {
            base.PrepareBenchmark(sesid, dbid, tableid, ref mappings);
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
        }

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
            using (var transaction = new Transaction(sesid))
            {
                var random = new Random();
                Api.JetSetCurrentIndex(sesid, tableid, nameof(DataRow.DoubleValue));
                for (var i = 0; i < DatabaseTestConsts.QueryCount; i++)
                {
                    double sum = 0.0;
                    Api.MakeKey(sesid, tableid, random.NextDouble(), MakeKeyGrbit.NewKey);
                    if (Api.TrySeek(sesid, tableid, SeekGrbit.SeekGT))
                    {
                        do
                        {
                            var row = EsentHelpers.LoadRow(sesid, tableid, ref mappings);
                            sum += row.DoubleValue;
                        } while (Api.TryMoveNext(sesid, tableid));
                    }
                }
                return DatabaseTestConsts.QueryCount;
            }
        }
    }
}