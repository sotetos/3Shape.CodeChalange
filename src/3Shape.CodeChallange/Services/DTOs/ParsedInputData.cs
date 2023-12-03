using Models;

namespace Services.DTOs
{
    public record ParsedInputData
    {
        private List<KeyValuePair<string, string>> _propertyValueData = new List<KeyValuePair<string, string>>();
        private List<string> _errors = new List<string>();

        public LibraryItemType InputType { get; set; }
        public IEnumerable<string> Errors => _errors;
        public IEnumerable<KeyValuePair<string, string>> PropertyValueData => _propertyValueData;
        public bool HasErrors => _errors.Any();

        public bool AddPropertyValueData(string propertyName, string value)
        {
            _propertyValueData.Add(new KeyValuePair<string, string>(propertyName, value));
            return true;
        }

        public bool AddError(string error)
        {
            _errors.Add(error);
            return true;
        }
    }
}
