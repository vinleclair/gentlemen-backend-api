using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gentlemen.Domain;
using Gentlemen.Infrastructure;
using Gentlemen.Infrastructure.Security;
using FluentValidation;
using Gentlemen.Infrastructure.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Features.Users
{
    public class Create
    {
        public class UserData
        {
            public string FirstName { get; set; }
            
            public string LastName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class UserDataValidator : AbstractValidator<UserData>
        {
            public UserDataValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly GentlemenContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public Handler(GentlemenContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }

            public async Task<UserEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                if (await _context.Customers.Where(x => x.Email == message.User.Email).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = Constants.IN_USE });
                }

                var salt = Guid.NewGuid().ToByteArray();
                var customer = new Customer
                {
                    FirstName = message.User.FirstName,
                    LastName = message.User.LastName,
                    Email = message.User.Email,
                    Hash = _passwordHasher.Hash(message.User.Password, salt),
                    Salt = salt
                };

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync(cancellationToken);
                var user = _mapper.Map<Customer, User>(customer);
                user.Token = await _jwtTokenGenerator.CreateToken();
                return new UserEnvelope(user);
            }
        }
    }
}