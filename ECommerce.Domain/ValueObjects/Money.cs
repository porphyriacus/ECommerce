using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Domain.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; }
        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0) throw new ArgumentException("Amount can't be negative");
            if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentException("Currency required");

            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }
        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot add different currencies");
            return new Money(Amount + other.Amount, Currency);
        }
    }
}
