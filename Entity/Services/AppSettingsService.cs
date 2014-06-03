using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Configuration;
using System.Net.Http;
using BombVacuum.Entity.Models;
using BombVacuum.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BombVacuum.Entity.Services
{
    public class SettingsService
    {
        private static readonly SettingsService _instance = new SettingsService();
        private readonly ApplicationDbContext _context;
        private Dictionary<string, string> _settings;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static SettingsService()
        {
        }

        public bool GoogleOauth
        {
            get
            {
                if (!_settings.ContainsKey("googleOauth")) return false;
                bool useOauth;
                bool.TryParse(_settings["googleOauth"], out useOauth);
                return useOauth;
            }
            set
            {
                Save("googleOauth", value.ToString());
                Load();
            }
        }
        public string GoogleOauthId
        {
            get
            {
                if (!_settings.ContainsKey("googleOauthId")) return null;
                return _settings["googleOauthId"];
            }
            set
            {
                Save("googleOauthId", value);
                Load();
            }
        }

        public string GoogleOauthSecret
        {
            get
            {
                if (!_settings.ContainsKey("googleOauthSecret")) return null;
                return _settings["googleOauthSecret"];
            }
            set
            {
                Save("googleOauthSecret", value);
                Load();
            }
        }

        private SettingsService()
        {
            _context = new ApplicationDbContext();
            Load();
        }

        private async void Load()
        {
            _settings = await _context.AppSettings.ToDictionaryAsync(k => k.Property, v => v.Value);
        }

        private async void Save(string key, string value)
        {
            var appsetting = await _context.AppSettings.FirstOrDefaultAsync(x => x.Property == key);
            if (appsetting != null)
            {
                appsetting.Value = value;
            }
            else
            {
                appsetting = new AppSetting
                {
                    Property = key,
                    Value = value
                };
                _context.AppSettings.Add(appsetting);
            }
            await _context.SaveChangesAsync();
        }

        public static SettingsService Instance
        {
            get { return _instance; }
        }
    }
}