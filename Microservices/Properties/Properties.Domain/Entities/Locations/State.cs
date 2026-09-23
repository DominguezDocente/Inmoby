using Properties.Domain.Exceptions;

namespace Properties.Domain.Entities.Locations
{
    public sealed class State
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid CountryId { get; private set; }
        public Country Country { get; private set; } = null!;
        public ICollection<City> Cities { get; private set; } = new List<City>();

        public State(string name, Guid countryId)
        {
            ApplyNameRules(name);
            ApplyCountryIdRules(countryId);

            Id = Guid.CreateVersion7();
            Name = name;
            CountryId = countryId;
        }

        public void UpdateCountryId(Guid countryId)
        {
            ApplyCountryIdRules(countryId);
            CountryId = countryId;
        }

        public void UpdateName(string name)
        {
            ApplyNameRules(name);
            Name = name;
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre del estado es requerido.");
            }

            if (name.Trim().Length < 3)
            {
                throw new BussinesRuleException("El nombre del estado debe tener al menos 3 carácteres.");
            }

            if (name.Trim().Length >= 64)
            {
                throw new BussinesRuleException("El nombre del estado debe tener máximo 64 carácteres.");
            }
        }

        private static void ApplyCountryIdRules(Guid countryId)
        {
            if (countryId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id del país es requerido.");
            }
        }
    }
}
