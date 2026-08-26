using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Common.ValueObjects
{
    public sealed class Currency
    {
        public decimal Amount { get; private set; }
        public CurrencyType Type { get; private set; } = null!;

        private Currency(decimal amount, CurrencyType type)
        {
            Amount = amount;
            Type = type;
        }

        public static Currency Create(decimal amount, CurrencyType type)
        {
            if (amount < 0)
            {
                throw new BussinesRuleException("El monto de la moneda no puede ser negativo.");
            }

            if (type is null)
            {
                throw new BussinesRuleException("El tipo de moneda es requerido.");
            }

            return new Currency(amount, type);
        }

        public static Currency Zero(CurrencyType type) => new Currency(0, type);

        public Currency ChangeAmount(decimal newAmount)
        {
            if (newAmount < 0)
            {
                throw new BussinesRuleException("El monto de la moneda no puede ser negativo.");
            }

            return new Currency(newAmount, Type);
        }

        public bool IsGreaterThan(Currency other) 
        {
            EnsureSameType(this, other);
            return Amount > other.Amount;
        }

        public bool IsLessThan(Currency other)
        {
            EnsureSameType(this, other);
            return Amount < other.Amount;
        }

        public static Currency operator + (Currency first, Currency second)
        {
            EnsureSameType(first, second);

            decimal result = first.Amount + second.Amount;

            if (result < 0)
            {
                throw new BussinesRuleException("El monto de la moneda no puede ser negativo.");
            }

            return new Currency(result, first.Type);
        }

        public static Currency operator - (Currency first, Currency second)
        {
            EnsureSameType(first, second);

            decimal result = first.Amount - second.Amount;

            if (result < 0)
            {
                throw new BussinesRuleException("El monto de la moneda no puede ser negativo.");
            }

            return new Currency(result, first.Type);
        }

        public static Currency operator * (Currency first, Currency second)
        {
            EnsureSameType(first, second);

            decimal result = first.Amount * second.Amount;

            if (result < 0)
            {
                throw new BussinesRuleException("El monto de la moneda no puede ser negativo.");
            }

            return new Currency(result, first.Type);
        }

        private static void EnsureSameType(Currency first, Currency second) 
        {
            if (!first.Type.Equals(second.Type))
            {
                throw new BussinesRuleException("No se pueden sumar monedas de diferentes tipos.");
            }
        }
    }
}
