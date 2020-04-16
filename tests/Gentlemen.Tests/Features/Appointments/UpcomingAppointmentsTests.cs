using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Gentlemen.Features.Appointments;
using Xunit;

namespace Gentlemen.Tests.Features.Appointments
{
    public class UpcomingAppointmentsTests : TestFixture
    {
        private const int Count = 5;

        [Fact]
        public async Task Expect_Fetch_Empty_Upcoming_Appointments()
        {
            var upcomingAppointmentsEnvelope = await AppointmentHelpers.FetchUpcomingAppointments(this, 1);

            Assert.Empty(upcomingAppointmentsEnvelope.UpcomingAppointments);
        }

        [Fact]
        public async Task Expect_Fetch_Many_Upcoming_Appointments()
        {
            for (int i = 0; i < Count; i++)
            {
                var command = new Create.Command()
                {
                    Appointment = new Create.AppointmentData()
                    {
                        ClientName = faker.Person.FullName,
                        ClientEmail = faker.Internet.Email(),
                        BarberId = 1,
                        Date = faker.Date.Future().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Time = faker.Date.Future().ToString("HH:mm", CultureInfo.InvariantCulture),
                        ServiceId = 1
                    }
                };

                await AppointmentHelpers.CreateAppointment(this, command);
            }

            var upcomingAppointmentsEnvelope = await AppointmentHelpers.FetchUpcomingAppointments(this, 1);

            Assert.Equal(Count, upcomingAppointmentsEnvelope.UpcomingAppointments.Values.Sum(list => list.Count));
        }

        [Fact]
        public async Task Expect_Fetch_One_Upcoming_Appointment()
        {
            var command = new Create.Command()
            {
                Appointment = new Create.AppointmentData()
                {
                    ClientName = faker.Person.FullName,
                    ClientEmail = faker.Internet.Email(),
                    BarberId = 1,
                    Date = faker.Date.Future().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Time = faker.Date.Future().ToString("HH:mm", CultureInfo.InvariantCulture),
                    ServiceId = 1
                }
            };

            await AppointmentHelpers.CreateAppointment(this, command);

            var upcomingAppointmentsEnvelope = await AppointmentHelpers.FetchUpcomingAppointments(this, 1);

            Assert.Single(upcomingAppointmentsEnvelope.UpcomingAppointments);
        }
    }
}