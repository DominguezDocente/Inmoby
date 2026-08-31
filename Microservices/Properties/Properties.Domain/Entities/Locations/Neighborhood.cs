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

        public Neighborhood(string name, Guid CityId)
        {
            ApplyNameRules(name);
            ApplyCityIdRules(CityId);
            Name = name;
            CityId = CityId;
        }

        public void UpdateCityId(Guid CityId)
        {
            ApplyCityIdRules(CityId);
            CityId = CityId;
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

        private void ApplyCityIdRules(Guid CityId)
        {
            if (CityId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id de la ciudad es requerido.");
            }
        }
    }
}
