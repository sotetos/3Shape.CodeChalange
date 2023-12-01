using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Internals.Models
{
    public record ParsedSearchCondition
    {
        public ParsedSearchCondition(string value)
        {
            StringValue = value;
            if (int.TryParse(value, out var intValue))
            {
                IntegerValue = intValue;
            }
        }

        public string StringValue { get; set; }
        public int? IntegerValue { get; set; }


    }
}
