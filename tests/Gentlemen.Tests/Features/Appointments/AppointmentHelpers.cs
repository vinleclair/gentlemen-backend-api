using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentlemen.Domain;
using Gentlemen.Features.Appointments;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Tests.Features.Appointments
{
    public static class AppointmentHelpers
    {
        public static async Task<Appointment> CreateAppointment(TestFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();

            var appointmentCreateHandler = new Create.Handler(dbContext);
            var created = await appointmentCreateHandler.Handle(command, new CancellationToken());

            var dbAppointment = await fixture.ExecuteDbContextAsync(db => db.Appointments
                .Where(a => a.AppointmentId == created.Appointment.AppointmentId)
                .SingleOrDefaultAsync());

            return dbAppointment;
        }

        public static async Task<UpcomingAppointmentsEnvelope> FetchUpcomingAppointments(TestFixture fixture,
            int barberId)
        {
            var dbContext = fixture.GetDbContext();

            var upcomingAppointmentsQueryHandler = new UpcomingAppointments.QueryHandler(dbContext);
            var fetched = await upcomingAppointmentsQueryHandler.Handle(new UpcomingAppointments.Query(barberId),
                new CancellationToken());

            Console.Write(fetched);
            return fetched;
        }
    }
}