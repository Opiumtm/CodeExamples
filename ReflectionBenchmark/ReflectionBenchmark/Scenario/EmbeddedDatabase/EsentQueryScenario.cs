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
        public override string ScenarioName => $"ESENT SUM {DatabaseTestConsts.QueryCount} Queries";

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        protected override object PrepareBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
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
            return null;
        }

        /// <summary>
        /// Do benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings, object data)
        {
            using (var transaction = new Transaction(sesid))
            {
                var random = new Random();
                Api.JetSetCurrentIndex(sesid, tableid, nameof(DataRow.DoubleValue));
                var buf = new byte[sizeof(double)];
                for (var i = 0; i < DatabaseTestConsts.QueryCount; i++)
                {
                    double sum = 0.0;
                    Api.MakeKey(sesid, tableid, random.NextDouble(), MakeKeyGrbit.NewKey);
                    if (Api.TrySeek(sesid, tableid, SeekGrbit.SeekGT))
                    {
                        do
                        {
                            int acts;
                            if (Api.JetRetrieveColumn(sesid, tableid, mappings.DoubleValue, buf, sizeof(double), 0, out acts, RetrieveColumnGrbit.RetrieveFromIndex, new JET_RETINFO()) < 0)
                            {
                                throw new InvalidOperationException();
                            }
                            sum += BitConverter.ToDouble(buf, 0);
                        } while (Api.TryMoveNext(sesid, tableid));
                    }
                }
                return DatabaseTestConsts.QueryCount;
            }
        }
    }

    /// <summary>
    /// ESENT Query scenario.
    /// </summary>
    public sealed class EsentPkSeekScenario : EsentScenarioBase
    {
        /// <summary>
        /// Scenario name.
        /// </summary>
        public override string ScenarioName => $"ESENT {DatabaseTestConsts.InsertCount} Primary Key Seek";

        /// <summary>
        /// Prepare benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        protected override object PrepareBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
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
            var seekArray = new int[DatabaseTestConsts.InsertCount];
            for (var i = 0; i < DatabaseTestConsts.InsertCount; i++)
            {
                seekArray[i] = random.Next(1, DatabaseTestConsts.InsertCount);
            }
            return seekArray;
        }

        /// <summary>
        /// Do benchmark.
        /// </summary>
        /// <param name="sesid">Session id.</param>
        /// <param name="dbid">Database id.</param>
        /// <param name="tableid">Table id.</param>
        /// <param name="mappings">Column mappings.</param>
        /// <returns>Iteration count.</returns>
        protected override int DoBenchmark(JET_SESID sesid, JET_DBID dbid, JET_TABLEID tableid, ref EsentColumnMappings mappings, object data)
        {
            var seekArray = (int[]) data;
            using (var transaction = new Transaction(sesid))
            {
                Api.JetSetCurrentIndex(sesid, tableid, nameof(DataRow.Id));
                double sum = 0.0;
                for (var i = 0; i < DatabaseTestConsts.InsertCount; i++)
                {
                    Api.MakeKey(sesid, tableid, seekArray[i], MakeKeyGrbit.NewKey);
                    if (Api.TrySeek(sesid, tableid, SeekGrbit.SeekEQ))
                    {
                        var row = EsentHelpers.LoadRow(sesid, tableid, ref mappings);
                        sum += row.DoubleValue;
                    }
                }
                return DatabaseTestConsts.InsertCount;
            }
        }
    }
}