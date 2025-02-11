﻿using ShellApp.Items;

namespace ShellApp.Items.Events
{
    public class ItemDeletedEvent : DomainEvent
    {
        public ItemDeletedEvent(string itemId)
        {
            ItemId = itemId;
        }

        public string ItemId { get; }
    }
}
