using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ProcessApi.Models;

namespace ProcessApi.Core
{
    /// <inheritdoc />
    public class ProcessInformationStorage : IProcessInformationStorage
    {
        private readonly ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();
        private ProcessInfo[] _processInfos = new ProcessInfo[0];
        
        /// <inheritdoc />
        public IEnumerable<ProcessInfo> GetProcessesInformation()
        {
            _readerWriterLock.EnterReadLock();
            var information = _processInfos;
            _readerWriterLock.ExitReadLock();
            return information;
        }

        /// <inheritdoc />
        public void SetProcessesInformation(IEnumerable<ProcessInfo> processInfos)
        {
            _readerWriterLock.EnterWriteLock();
            _processInfos = processInfos.ToArray();
            _readerWriterLock.ExitWriteLock();
        }
    }
}