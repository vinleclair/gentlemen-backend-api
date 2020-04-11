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
                    Name = "John",
                    Email = "john.doe@example.com"
                }
            };

            var appointment = await AppointmentHelpers.CreateAppointment(this, command);

            Assert.NotNull(appointment);
            Assert.AreEqual(appointment.Name, command.Appointment.Name);
            Assert.AreEqual(appointment.Email, command.Appointment.Email);
        }
    }
}