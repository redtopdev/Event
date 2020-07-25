// <copyright file="Program.cs" company="RedTop">
// RedTop
// </copyright>

namespace Evento.Service
{
    public static class Program
    {
        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">Command line or service arguments</param>
        public static void Main(string[] args)
        {
            Engaze.Core.Web.EngazeWebHost.Run<Startup>(args);
        }
    }
}
