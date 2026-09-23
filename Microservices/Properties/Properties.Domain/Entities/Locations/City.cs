using Properties.Domain.Exceptions;

namespace Properties.Domain.Entities.Locations
{
    public sealed class City
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = null!;
        public Guid StateId { get; private set; }
        public State State { get; private set; } = null!;
        public ICollection<Neighborhood> Neighborhoods { get; private set; } = new List<Neighborhood>();

        public City(string name, Guid stateId)
        {
            ApplyNameRules(name);
            ApplyStateIdRules(stateId);

            Id = Guid.CreateVersion7();
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

        private static void ApplyNameRules(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new BussinesRuleException("El nombre de la ciudad es requerido.");
            }

            if (name.Trim().Length < 3)
            {
                throw new BussinesRuleException("El nombre de la ciudad debe tener al menos 3 carácteres.");
            }

            if (name.Trim().Length >= 64)
            {
                throw new BussinesRuleException("El nombre de la ciudad debe tener máximo 64 carácteres.");
            }
        }

        private static void ApplyStateIdRules(Guid stateId)
        {
            if (stateId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id del estado es requerido.");
            }
        }
    }
}
