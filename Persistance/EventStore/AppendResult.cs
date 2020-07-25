// <copyright file="AppendResult.cs" company="RedTop">
// RedTop
// </copyright>

namespace Evento.DataPersistance
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
