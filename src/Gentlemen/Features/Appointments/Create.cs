﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Domain;
using Gentlemen.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Features.Appointments
{
    public class Create
    {
        public class AppointmentData
        {
            public string Name { get; set; }

            public string Email { get; set; }
        }

        public class AppointmentDataValidator : AbstractValidator<AppointmentData>
        {
            public AppointmentDataValidator()
            {
                RuleFor(x => x.Name).NotNull().NotEmpty();
                RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
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
                var appointment = new Appointment()
                {
                    Name = message.Appointment.Name,
                    Email = message.Appointment.Email,
                };
                await _context.Appointments.AddAsync(appointment, cancellationToken);
                
                await _context.SaveChangesAsync(cancellationToken);

                return new AppointmentEnvelope(appointment);
            }
        }
    }
}