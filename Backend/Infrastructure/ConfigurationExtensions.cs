﻿using Microsoft.Extensions.Configuration;

namespace ShellApp.Infrastructure
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionString(this IConfiguration configuration, string name, string database)
        {
            var connectionString = configuration.GetConnectionString(name);
            return $"{connectionString};Database={database}";
        }
    }
}
