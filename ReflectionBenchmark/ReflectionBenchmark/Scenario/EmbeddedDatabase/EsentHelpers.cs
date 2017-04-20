using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Isam.Esent.Interop;

namespace ReflectionBenchmark.EmbeddedDatabase
{
    public static class EsentHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DataRow LoadRow(JET_SESID sesid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
        {
            var idColumn = new Int32ColumnValue() {Columnid = mappings.Id};
            var stringColumn = new StringColumnValue() {Columnid = mappings.StringValue};
            var doubleColumn = new DoubleColumnValue() {Columnid = mappings.DoubleValue};
            Api.RetrieveColumns(sesid, tableid, idColumn, stringColumn, doubleColumn);
            return new DataRow
            {
                Id = idColumn.Value ?? 0,
                StringValue = stringColumn.Value,
                DoubleValue = doubleColumn.Value ?? 0.0
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateRow(DataRow row, JET_SESID sesid, JET_TABLEID tableid, ref EsentColumnMappings mappings)
        {
            Api.SetColumns(sesid, tableid,
                new StringColumnValue() { Columnid = mappings.StringValue, Value = row.StringValue },
                new DoubleColumnValue() { Columnid = mappings.DoubleValue, Value = row.DoubleValue }
            );
        }

        public static (JET_TABLEID tableid, JET_DBID dbid, EsentColumnMappings mappings) InitializeDb(JET_SESID sesid, string databasePath)
        {
            JET_DBID dbid;
            JET_TABLEID tableid;
            EsentColumnMappings mappings = default(EsentColumnMappings);
            Api.CreateDatabase(sesid, databasePath, out dbid, CreateDatabaseGrbit.OverwriteExisting);
            Api.JetCreateTable(sesid, dbid, "test", 1, 100, out tableid);
            Api.JetAddColumn(sesid, tableid, nameof(DataRow.Id), new JET_COLUMNDEF()
            {
                coltyp = JET_coltyp.Long,
                grbit = ColumndefGrbit.ColumnAutoincrement | ColumndefGrbit.ColumnNotNULL
            }, null, 0, out mappings.Id);
            Api.JetAddColumn(sesid, tableid, nameof(DataRow.StringValue), new JET_COLUMNDEF()
            {
                coltyp = JET_coltyp.LongText,
                grbit = ColumndefGrbit.ColumnMaybeNull,
                cp = JET_CP.Unicode
            }, null, 0, out mappings.StringValue);
            Api.JetAddColumn(sesid, tableid, nameof(DataRow.DoubleValue), new JET_COLUMNDEF()
            {
                coltyp = JET_coltyp.IEEEDouble,
                grbit = ColumndefGrbit.ColumnNotNULL
            }, null, 0, out mappings.DoubleValue);
            var idDef = $"+{nameof(DataRow.Id)}\0\0";
            var stringDef = $"+{nameof(DataRow.StringValue)}\0\0";
            var doubleDef = $"+{nameof(DataRow.DoubleValue)}\0\0";
            Api.JetCreateIndex(sesid, tableid, nameof(DataRow.Id), CreateIndexGrbit.IndexPrimary, idDef, idDef.Length, 100);
            Api.JetCreateIndex(sesid, tableid, nameof(DataRow.StringValue), 0, stringDef, stringDef.Length, 100);
            Api.JetCreateIndex(sesid, tableid, nameof(DataRow.DoubleValue), CreateIndexGrbit.IndexDisallowNull, doubleDef, doubleDef.Length, 100);
            return (tableid, dbid, mappings);
        }

        public static (Instance instance, Session session) OpenSession(string dbFolder)
        {
            Instance instance = new Instance("testinstance", "testinstance")
            {
                Parameters =
                {
                   SystemDirectory = dbFolder,
                   TempDirectory = dbFolder,
                   LogFileDirectory = dbFolder,
                   Recovery = true,
                   CircularLog = false,
                }
            };
            instance.Init();
            Session session = new Session(instance);
            return (instance, session);
        }
    }

    public struct EsentColumnMappings
    {
        public JET_COLUMNID Id;
        public JET_COLUMNID StringValue;
        public JET_COLUMNID DoubleValue;
    }
}