﻿using System.Collections.Generic;
using System.Linq;
using ShellApp.Items.Queries;
using ShellApp.Items.Domain.Entities;
using System;
using ShellApp.Items.Commands;

namespace ShellApp.Items.Application
{
    public static class Mappings
    {
        public static void Map(CreateItemCommand src, Item item)
        {
            item.Text = src.Text;
            item.Description = src.Description;
        }

        public static void Map(UpdateItemCommand src, Item item)
        {
            item.Text = src.Text;
            item.Description = src.Description;
        }

        public static ItemDto Map(Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Text = item.Text,
                Description = item.Description,
                PictureUri = item.PictureUri,
                Created = item.Created,
                CreatedBy = item.CreatedBy,
                LastModified = item.LastModified,
                LastModifiedBy = item.LastModifiedBy
            };
        }
    }
}
