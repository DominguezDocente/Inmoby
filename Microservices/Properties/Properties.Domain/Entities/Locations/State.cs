using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Locations
{
    public sealed class State
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid CountryId { get; private set; }
        public Country Country { get; private set; }

        public State(string name, Guid countryId)
        {
            ApplyNameRules(name);
            ApplyCountryIdRules(CountryId);
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

        private void ApplyNameRules(string name)
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

        private void ApplyCountryIdRules(Guid countryId)
        {
            if (countryId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id del país es requerido.");
            }
        }
    }
}
