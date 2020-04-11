using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Gentlemen.Features.Appointments;

namespace Gentlemen.IntegrationTests.Features.Appointments
{
    public static class AppointmentHelpers
    {
        public static async Task<Domain.Appointment> CreateAppointment(SliceFixture fixture, Create.Command command)
        {
            var dbContext = fixture.GetDbContext();

            var appointmentCreateHandler = new Create.Handler(dbContext);
            var created = await appointmentCreateHandler.Handle(command, new System.Threading.CancellationToken());

            var dbAppointment = await fixture.ExecuteDbContextAsync(db => db.Appointments.Where(a => a.AppointmentId == created.Appointment.AppointmentId)
                .SingleOrDefaultAsync());

            return dbAppointment;
        }
    }
}