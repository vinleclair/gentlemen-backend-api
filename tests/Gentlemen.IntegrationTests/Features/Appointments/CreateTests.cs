using System;
using System.Globalization;
using System.Threading.Tasks;
using Gentlemen.Features.Appointments;
using NUnit.Framework;

namespace Gentlemen.IntegrationTests.Features.Appointments
{
    [TestFixture]
    public class CreateTests : TestFixture
    {
        [Test]
        public async Task Expect_Create_Appointment()
        {
            var command = new Create.Command()
            {
                Appointment = new Create.AppointmentData()
                {
                    ClientName = faker.Person.FullName,
                    ClientEmail = faker.Internet.Email(),
                    BarberId = faker.Random.Number(1, 2),
                    Date = faker.Date.Future().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Time = faker.Date.Future().ToString("HH:mm", CultureInfo.InvariantCulture),
                    ServiceId = faker.Random.Number(1, 2),
                }
            };

            var appointment = await AppointmentHelpers.CreateAppointment(this, command);

            Assert.NotNull(appointment);
            Assert.AreEqual(appointment.ClientName, command.Appointment.ClientName);
            Assert.AreEqual(appointment.ClientEmail, command.Appointment.ClientEmail);
            Assert.AreEqual(appointment.BarberId, command.Appointment.BarberId);
            Assert.AreEqual(appointment.ScheduledDate, DateTime.ParseExact(command.Appointment.Date + " " + command.Appointment.Time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            Assert.AreEqual(appointment.ServiceId, command.Appointment.ServiceId);
        }
    }
}