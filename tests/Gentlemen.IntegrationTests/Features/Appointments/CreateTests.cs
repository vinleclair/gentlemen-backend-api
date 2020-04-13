using System.Threading.Tasks;
using Gentlemen.Features.Appointments;
using NUnit.Framework;

namespace Gentlemen.IntegrationTests.Features.Appointments
{
    [TestFixture]
    public class CreateTests : SliceFixture
    {
        [Test]
        public async Task Expect_Create_Appointment()
        {
            var command = new Create.Command()
            {
                Appointment = new Create.AppointmentData()
                {
                    ClientName = "John",
                    ClientEmail = "john.doe@example.com",
                    BarberId = 5
                }
            };

            var appointment = await AppointmentHelpers.CreateAppointment(this, command);

            Assert.NotNull(appointment);
            Assert.AreEqual(appointment.ClientName, command.Appointment.ClientName);
            Assert.AreEqual(appointment.ClientEmail, command.Appointment.ClientEmail);
        }
    }
}