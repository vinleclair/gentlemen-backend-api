using System.Collections.Generic;
using Gentlemen.Domain;

namespace Gentlemen.Features.Barbers
{
    public class BarbersEnvelope
    {
        public List<Barber> Barbers { get; set; }
    }
}