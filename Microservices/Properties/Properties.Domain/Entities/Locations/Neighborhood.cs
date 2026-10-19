using Properties.Domain.Entities.Properties;
using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Locations
{
    public sealed class Neighborhood
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid CityId { get; private set; }
        public City City { get; private set; }
        public ICollection<Property>? Properties { get; private set; } = new List<Property>();

        public Neighborhood(string name, Guid cityId)
        {
            ApplyNameRules(name);
            ApplyCityIdRules(cityId);
            Name = name;
            CityId = cityId;
        }

        public void UpdateCityId(Guid cityId)
        {
            ApplyCityIdRules(cityId);
            CityId = cityId;
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

        private void ApplyCityIdRules(Guid cityId)
        {
            if (cityId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id de la ciudad es requerido.");
            }
        }
    }
}
