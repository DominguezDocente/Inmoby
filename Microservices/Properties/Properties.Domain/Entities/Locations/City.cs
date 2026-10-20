using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Locations
{
    public sealed class City
    {

        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid StateId { get; private set; }
        public State State { get; private set; }
        public ICollection<Neighborhood> Neighborhoods { get; private set; } = new List<Neighborhood>();

        public City(string name, Guid stateId)
        {
            ApplyNameRules(name);
            ApplyStateIdRules(stateId);
            Name = name;
            StateId = stateId;
        }

        public void UpdateStateId(Guid stateId)
        {
            ApplyStateIdRules(stateId);
            StateId = stateId;
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

        private void ApplyStateIdRules(Guid stateId)
        {
            if (stateId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id del estado es requerido.");
            }
        }
    }
}
