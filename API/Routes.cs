// <copyright file="Routes.cs" company="RedTop">
// RedTop
// </copyright>

namespace Evento.Service
{
    public static class Routes
    {
        public const string Evento = "/evento";

        public const string DeleteEvento = "evento/{eventId}";

        public const string LeaveEvento = "evento/{eventId}/participant/{participantId}/leave";

        public const string EndEvento = "evento/{eventId}/end";

        public const string ExtendEvento = "evento/{eventId}/extend/{endTime}";

        public const string EventoParticipants = "evento/{eventId}/participants";

        public const string EventoParticipantState = "evento/{eventId}/participant/{participantId}/state/{state}";

        public const string ServiceStatus = "evento/service-status";
    }
}
