
using System;

namespace Engaze.Evento.Core
{
    public class BaseCommand : ICommand
    {
        public Guid Id { get; private set; }
    }
}
