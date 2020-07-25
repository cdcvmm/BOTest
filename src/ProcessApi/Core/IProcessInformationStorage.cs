using System.Collections.Generic;
using ProcessApi.Models;

namespace ProcessApi.Core
{
    /// <summary>
    /// Thread safe storage for information about calculated indicators of all processes in the system 
    /// </summary>
    public interface IProcessInformationStorage
    {
        /// <summary>
        /// Return information about processes
        /// </summary>
        IEnumerable<ProcessInfo> GetProcessesInformation();

        /// <summary>
        /// Set information about processes in storage
        /// </summary>
        void SetProcessesInformation(IEnumerable<ProcessInfo> processInfos);
    }
}