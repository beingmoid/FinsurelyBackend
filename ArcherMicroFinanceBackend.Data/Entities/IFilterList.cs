using System;
using System.Collections.Generic;
using System.Text;

namespace PanoramaBackend.Data.Entities
{
    public interface IFilterList<T>
    {
        public T Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
  
}
