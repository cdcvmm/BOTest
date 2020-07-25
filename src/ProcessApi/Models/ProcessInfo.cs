namespace ProcessApi.Models
{
    /// <summary>
    /// Information about process in system
    /// </summary>
    public class ProcessInfo
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int ProcessId { get; set; }
            
        /// <summary>
        /// Name of process
        /// </summary>
        public string ProcessName { get; set; }
            
        /// <summary>
        /// Current memory usage in bytes
        /// </summary>
        public long MemoryUsage { get; set; }
            
        /// <summary>
        /// Current cpu usage in milliseconds for the last 500 milliseconds
        /// </summary>
        public double CpuUsage { get; set; }
    }
}