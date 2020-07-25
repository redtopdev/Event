// <copyright file="AppendResult.cs" company="RedTop">
// RedTop
// </copyright>

namespace Engaze.Event.DataPersistence.EventStore
{
    public class AppendResult
    {
        public AppendResult(long nextExpectedVersion)
        {
            NextExpectedVersion = nextExpectedVersion;
        }

        public long NextExpectedVersion { get; }
    }
}
