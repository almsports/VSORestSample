using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSample.DataModel
{
    public class ApiCollection<T>
    {
        [JsonProperty(PropertyName = "value")]
        public IEnumerable<T> Value { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }

        public T GetIndex(IEnumerable<T> collection, int index)
        {
            int number = 0;
            foreach (var item in collection)
            {
                if (Convert.ToInt32(number) == index)
                {
                    return item;
                }
                number++;
            }

            return default(T);
        }
    }
}
