using System;

namespace Model
{
    public class City
    {
        public int PostalCode { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public City(int postalCode, string name, Country country)
        {
            this.PostalCode = postalCode;
            this.Name = name;
            this.Country = country;
        }

        public City()
        {
            this.Country = new Country();
        }
    }
}