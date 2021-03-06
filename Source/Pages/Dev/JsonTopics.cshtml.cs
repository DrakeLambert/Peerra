using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DrakeLambert.Peerra.Data;
using DrakeLambert.Peerra.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DrakeLambert.Peerra.Pages.Dev
{
    public class JsonTopicsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public JsonTopicsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public string JsonTopics { get; set; }

        public async Task<IActionResult> OnGet([FromQuery] bool raw = false)
        {
            var topics = (await _context.Topics.ToListAsync()).Where(topic => topic.ParentId == null);

            var jsonResolver = new PropertyIgnoreContractResolver();
            jsonResolver.Ignore<Topic>("Parent", "ParentId");

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = jsonResolver;

            JsonTopics = JsonConvert.SerializeObject(topics, Formatting.Indented, serializerSettings);

            if (raw)
            {
                return Content(JsonTopics);
            }
            else
            {
                return Page();
            }
        }

        private class PropertyIgnoreContractResolver : DefaultContractResolver
        {
            private readonly Dictionary<Type, HashSet<string>> _ignores = new Dictionary<Type, HashSet<string>>();

            public void Ignore<T>(params string[] jsonPropertyNames)
            {
                var type = typeof(T);

                if (!_ignores.ContainsKey(type))
                    _ignores[type] = new HashSet<string>();

                foreach (var prop in jsonPropertyNames)
                    _ignores[type].Add(prop);
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);

                if (IsIgnored(property.DeclaringType, property.PropertyName))
                {
                    property.ShouldSerialize = i => false;
                    property.Ignored = true;
                }

                return property;
            }

            private bool IsIgnored(Type type, string jsonPropertyName)
            {
                if (!_ignores.ContainsKey(type))
                    return false;

                return _ignores[type].Contains(jsonPropertyName);
            }
        }
    }
}
