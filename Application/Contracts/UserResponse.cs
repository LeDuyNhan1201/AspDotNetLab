using System;

namespace Application.Contracts
{
    public sealed record UserResponse
    {

        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

    }
}
