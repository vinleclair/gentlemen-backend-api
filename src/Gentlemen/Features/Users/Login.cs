using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gentlemen.Infrastructure;
using Gentlemen.Infrastructure.Errors;
using Gentlemen.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gentlemen.Features.Users
{
    public class Login
    {
        public class UserData
        {
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
                var customer = await _context.Customers.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync(cancellationToken);
                if (customer == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                if (!customer.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, customer.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                var user = _mapper.Map<Domain.Customer, User>(customer);
                user.Token = await _jwtTokenGenerator.CreateToken();
                return new UserEnvelope(user);
            }
        }
    }
}