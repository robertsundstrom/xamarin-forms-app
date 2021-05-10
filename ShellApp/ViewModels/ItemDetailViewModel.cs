﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ShellApp.Client;
using ShellApp.Services;
using Xamarin.Forms;

namespace ShellApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;
        private Command deleteItemCommand;

        public ItemDetailViewModel(IDataStore<Item> dataStore)
        {
            DataStore = dataStore;
        }

        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public IDataStore<Item> DataStore { get; }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public Command DeleteItemCommand => deleteItemCommand ??= new Command(async () =>
        {
            var items = await DataStore.DeleteItemAsync(ItemId);

            await Shell.Current.Navigation.PopAsync();
        });
    }
}
