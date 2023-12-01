using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.Internals.Models
{
    public record ParsedInputData
    {
        private List<KeyValuePair<string, string>> _propertyValueData = new List<KeyValuePair<string, string>>();
        private List<string> _errors = new List<string>();

        public LibraryItemType InputType { get; set; }
        public IEnumerable<string> Errors => _errors;
        public IEnumerable<KeyValuePair<string, string>> PropertyValueData => _propertyValueData;
        public bool HasErrors => _errors.Any();

        public bool AddPropertyValueData(KeyValuePair<string, string> data)
        {
            _propertyValueData.Add(data);
            return true;
        }

        public bool AddError(string error)
        {
            _errors.Add(error);
            return true;
        }
    }
}
