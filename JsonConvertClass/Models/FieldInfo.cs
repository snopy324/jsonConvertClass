using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonConvertClass.Models
{
    public class FieldInfo
    {
        public string Name { get; set; }
        public FieldInfo[] innerField { get; set; }
        public bool isClass { get; set; }
        public Type Type { get; set; }
    }
}
