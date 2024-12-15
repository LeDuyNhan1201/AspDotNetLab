using System;

namespace Application.Contracts
{
    public sealed record PreviewBookResponse
    {

        public Guid Id { get; set; }

        public string Title { get; set; }

    }
}
