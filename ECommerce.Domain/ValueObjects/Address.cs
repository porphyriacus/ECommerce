using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.ValueObjects
{
    public record Address
    {
        public string Street { get; init; }
        public string City { get; init; }
        public int? PostalCode { get; init; }
        public int HouseNumber { get; init; }

        public Address(string street, string city, int? postalCode, int houseNumber)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street is required");
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City is required");
            if (houseNumber <= 0)
                throw new ArgumentException("House number must be positive");

            Street = street;
            City = city;
            PostalCode = postalCode;
            HouseNumber = houseNumber;
        }
    }
}
