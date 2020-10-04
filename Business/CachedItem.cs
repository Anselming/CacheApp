using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class CachedItem
    {
        public CachedItem(object value)
        {
            this.Value = value;
            this.ValueType = value.GetType();
        }
        public object Value { get; set; }
        public Type ValueType { get; set; }
    }
}
