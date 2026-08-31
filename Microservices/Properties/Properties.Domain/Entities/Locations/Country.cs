using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Locations
{
    public sealed class Country
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;

        public Country(string name)
        {
            ApplyNameRules(name);
            Name = name;
            Id = Guid.CreateVersion7();
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
                throw new BussinesRuleException("El nombre del país es requerido.");
            }

            if (name.Trim().Length < 3)
            {
                throw new BussinesRuleException("El nombre del país debe tener al menos 3 carácteres.");
            }

            if (name.Trim().Length >= 64)
            {
                throw new BussinesRuleException("El nombre del país debe tener máximo 64 carácteres.");
            }
        }
    }
}
