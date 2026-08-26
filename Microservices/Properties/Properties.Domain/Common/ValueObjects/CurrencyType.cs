using Properties.Domain.Exceptions;

namespace Properties.Domain.Common.ValueObjects
{
    public class CurrencyType
    {
        public string Code { get; }

        public static readonly CurrencyType COP = new CurrencyType("COP");
        public static readonly CurrencyType USD = new CurrencyType("USD");
        public static readonly CurrencyType EUR = new CurrencyType("EUR");

        private static IReadOnlyCollection<CurrencyType> _all = new List<CurrencyType> { COP, USD, EUR }.AsReadOnly();

        public static IReadOnlyCollection<CurrencyType> All => _all;

        private CurrencyType(string code)
        {
            Code = code;
        }

        public static CurrencyType FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BussinesRuleException("El código del tipo de moneda es requierido.");
            }

            string normalized = code.Trim()
                                    .ToUpper();

            CurrencyType? currencyType = _all.FirstOrDefault(c => c.Code == normalized);

            if (currencyType is null)
            {
                throw new BussinesRuleException("Tipo de moneda inválido.");
            }

            return currencyType;
        }

        public bool Equals(CurrencyType? other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code;
        }

    }
}
