using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Properties
{
    public sealed class PropertyType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public string? Description { get; private set; } = null!;

        public PropertyType(string name, string? description = null)
        {
            ApplyNameRules(name);
            ApplyDescriptionRules(description);

            Id = Guid.CreateVersion7();
            Name = name;
            Description = description;
        }

        public void Update(string name, string? description = null)
        {
            ApplyNameRules(name);
            ApplyDescriptionRules(description);

            Name = name;
            Description = description;
        }

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre del tipo de inmueble es requerido.");
            }

            if (name.Trim().Length < 2)
            {
                throw new BussinesRuleException("El nombre del tipo de inmueble debe tener más de 2 carácteres.");
            }

            if (name.Trim().Length < 64)
            {
                throw new BussinesRuleException("El nombre del tipo de inmueble debe tener máximo 64 carácteres.");
            }
        }

        private static void ApplyDescriptionRules(string description)
        {
            if (description is not null && description.Trim().Length > 1024)
            {
                throw new BussinesRuleException("La descripción del tipo de inmueble debe tener máximo 1024 carácteres.");
            }
        }

    }
}
