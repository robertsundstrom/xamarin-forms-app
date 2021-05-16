﻿using MediatR;

namespace ShellApp.Commands
{
    public class UpdateItemCommand : IRequest<string>
    {
        public string ItemId { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
