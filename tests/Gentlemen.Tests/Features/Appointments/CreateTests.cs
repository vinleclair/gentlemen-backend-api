using System;
using System.Globalization;
using System.Threading.Tasks;
using Gentlemen.Features.Appointments;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Gentlemen.Tests.Features.Appointments
{
    public class CreateTests : TestFixture
    {
        [Fact]
        public async Task Expect_Create_Article()
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

            var appointment = await AppointmentHelpers.CreateAppointment(this, command);

            Assert.NotNull(appointment);
            Assert.Equal(appointment.ClientName, command.Appointment.ClientName);
            Assert.Equal(appointment.ClientEmail, command.Appointment.ClientEmail);
            Assert.Equal(appointment.BarberId, command.Appointment.BarberId);
            Assert.Equal(appointment.ScheduledDate, DateTime.ParseExact(command.Appointment.Date + " " + command.Appointment.Time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture));
            Assert.Equal(appointment.ServiceId, command.Appointment.ServiceId);
        }

        [Fact]
        public async Task Expect_Foreign_Key_Constraint_Fails()
        {
            var command = new Create.Command()
            {
                Appointment = new Create.AppointmentData()
                {
                    ClientName = faker.Person.FullName,
                    ClientEmail = faker.Internet.Email(),
                    BarberId = 99,
                    Date = faker.Date.Future().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Time = faker.Date.Future().ToString("HH:mm", CultureInfo.InvariantCulture),
                    ServiceId = 99
                }
            };
            
            await Assert.ThrowsAsync<DbUpdateException>(() => AppointmentHelpers.CreateAppointment(this, command));
        }

        [Fact]
        public async Task Expect_DateTime_In_Past_Exception_Thrown()
        {
            var command = new Create.Command()
            {
                Appointment = new Create.AppointmentData()
                {
                    ClientName = faker.Person.FullName,
                    ClientEmail = faker.Internet.Email(),
                    BarberId = 1,
                    Date = faker.Date.Recent().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Time = faker.Date.Recent().ToString("HH:mm", CultureInfo.InvariantCulture),
                    ServiceId = 1
                }
            };
            
            await Assert.ThrowsAsync<Exception>(() => AppointmentHelpers.CreateAppointment(this, command));
        }

    }
}