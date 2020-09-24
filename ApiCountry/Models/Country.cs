using System;
using System.Collections.Generic;

namespace ApiCountry.Models
{
    public class Country
    {
        public Guid Id { set; get; }
        public string Name { set; get; }
        public string PhotoUrl { set; get; }
        public List<State> States { set; get; }
    }
}
