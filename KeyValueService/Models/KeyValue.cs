using System.ComponentModel.DataAnnotations;

namespace KeyValueService.Models
{
    public class KeyValue
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}