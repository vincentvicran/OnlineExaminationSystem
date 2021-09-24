﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Web
{
    public static class ConfigurationManager
    {
        private static IConfiguration configuration = null;
        static ConfigurationManager()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();
        }
        public static string GetFilePath()
        {
            return configuration["CustomKeys:BaseUrl"] + "/file/";
        }
    }
}
