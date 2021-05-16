﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using ShellApp.Client;

namespace ShellApp.Services
{
    public class AzureDataStore : IDataStore<Item>
    {
        IEnumerable<Item> items;
        private readonly IItemsClient client;

        public AzureDataStore(IItemsClient client)
        {
            this.client = client;
            items = new List<Item>();
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;
        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                items = await client.GetItemsAsync(10, 0);
            }

            return items;
        }

        public async Task<Item> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                return await client.GetItemAsync(id);
            }

            return null;
        }

        public async Task<bool> CreateItemAsync(string text, string description)
        {
            if (!IsConnected)
                return false;

            var response = await client.CreateItemAsync(text, description);

            return true;
        }

        public async Task<bool> UpdateItemAsync(string id, string text, string description)
        {
            if (id == null || !IsConnected)
                return false;

            await client.UpdateItemAsync(id, text, description);

            return true;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            await client.DeleteItemAsync(id);

            return true;
        }
    }
}
