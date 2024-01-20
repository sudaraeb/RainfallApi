namespace RainfallApi.Data
{
    /// <summary>
    /// Represents a rainfall reading.
    /// </summary>
    public class RainfallReading
    {
        /// <summary>
        /// Gets or sets the date when the measurement was taken.
        /// </summary>
        public DateTime DateMeasured { get; set; }

        /// <summary>
        /// Gets or sets the amount of rainfall measured.
        /// </summary>
        public double AmountMeasured { get; set; }
    }
    /// <summary>
    /// Represents a response containing a list of rainfall readings.
    /// </summary>
    public class RainfallReadingResponse
    {
        /// <summary>
        /// Gets or sets the list of rainfall readings.
        /// </summary>
        public List<RainfallReading> Readings { get; set; }
    }
}
