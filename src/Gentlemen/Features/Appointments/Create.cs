using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Domain;
using Gentlemen.Infrastructure;
using FluentValidation;
using MediatR;


namespace Gentlemen.Features.Appointments
{
    public class Create
    {
        public class AppointmentData
        {
            public string ClientName { get; set; }

            public string ClientEmail { get; set; }
            
            public int BarberId { get; set; }
            
            public string Date { get; set; }
            
            public string Time { get; set; }
            
            public int ServiceId { get; set; }
        }

        public class AppointmentDataValidator : AbstractValidator<AppointmentData>
        {
            public AppointmentDataValidator()
            {
                RuleFor(x => x.ClientName).NotNull().NotEmpty();
                RuleFor(x => x.ClientEmail).NotNull().NotEmpty().EmailAddress();
                RuleFor(x => x.BarberId).NotNull().NotEmpty();
                RuleFor(x => x.Date).NotNull().NotEmpty();
                RuleFor(x => x.Time).NotNull().NotEmpty();
                RuleFor(x => x.ServiceId).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<AppointmentEnvelope>
        {
            public AppointmentData Appointment { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Appointment).NotNull().SetValidator(new AppointmentDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, AppointmentEnvelope>
        {
            private readonly GentlemenContext _context;

            public Handler(GentlemenContext context)
            {
                _context = context;
            }

            public async Task<AppointmentEnvelope> Handle(Command message, CancellationToken cancellationToken)
            { 
                var scheduledDate = DateTime.ParseExact(message.Appointment.Date + " " + message.Appointment.Time,
                    "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture);
                
                var appointment = new Appointment()
                {
                    ClientName = message.Appointment.ClientName,
                    ClientEmail = message.Appointment.ClientEmail,
                    BarberId = message.Appointment.BarberId,
                    ScheduledDate = scheduledDate,
                    ServiceId = message.Appointment.ServiceId,
                };
                await _context.Appointments.AddAsync(appointment, cancellationToken);
                
                await _context.SaveChangesAsync(cancellationToken);
                
                return new AppointmentEnvelope(appointment);
            }
        }
    }
}