using JsonConvertClass.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JsonConvertClass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JsonParserController : ControllerBase
    {
        int _count = 0;
        int count { get { return ++_count; } }
        Dictionary<string, Type> classInfo = new Dictionary<string, Type>();
        public JsonParserController()
        {

        }

        [HttpPost]
        public async Task<string> toCSharpClass(JsonElement json)
        {
            var info = GetFieldInfo("Root", json);
            return ToClass(info);
        }

        private FieldInfo GetFieldInfo(string name, JsonElement json)
        {
            var info = new FieldInfo();
            info.Name = name;
            info.isClass = json.ValueKind == JsonValueKind.Object;

            switch (json.ValueKind)
            {
                case JsonValueKind.Object:
                    var a = json.EnumerateObject();
                    info.innerField = a.Select(i => GetFieldInfo(i.Name, i.Value)).ToArray();
                    info.Type = typeof(object);
                    break;
                case JsonValueKind.Number:
                    info.Type = typeof(int);
                    break;
                case JsonValueKind.String:
                    info.Type = typeof(string);
                    break;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    info.Type = typeof(bool);
                    break;
                case JsonValueKind.Array:
                    info.Type = typeof(List<>);
                    break;
                case JsonValueKind.Null:
                case JsonValueKind.Undefined:
                default:
                    info.Type = typeof(object);
                    break;
            }

            return info;
        }

        private string ToClass(FieldInfo info, int deep = 0)
        {
            var sb = new StringBuilder();
            string prefix = new string('\t', deep);
            sb.AppendLine($"{prefix}{info.Name} {info.Type.Name}");
            if (info.isClass)
            {
                foreach (var inner in info.innerField)
                {
                    sb.Append(ToClass(inner, deep + 1));
                }
            }

            return sb.ToString();
        }
    }
}
