namespace ReflectionBenchmark.EmbeddedDatabase
{
    /// <summary>
    /// Data row to test.
    /// </summary>
    public class DataRow
    {
        /// <summary>
        /// Record Id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// String value.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Double value.
        /// </summary>
        public double DoubleValue { get; set; }
    }
}