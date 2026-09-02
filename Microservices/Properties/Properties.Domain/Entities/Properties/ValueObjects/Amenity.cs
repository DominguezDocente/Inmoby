using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Properties.Domain.Entities.Properties.ValueObjects
{
    public sealed record Amenity
    {
        public string Code { get; }
        public string Name { get; }

        public static readonly Amenity Jacuzzi = new Amenity("JACUZZI", "Jacuzzi");
        public static readonly Amenity Pool = new Amenity("POOL", "Piscina");
        public static readonly Amenity Gym = new Amenity("GYM", "Gimnasio");
        public static readonly Amenity Elevator = new Amenity("ELEVATOR", "Ascensor");
        public static readonly Amenity Garden = new Amenity("GARDEN", "Jardín");
        public static readonly Amenity Terrace = new Amenity("TERRACE", "Terraza");
        public static readonly Amenity AirConditioning = new Amenity("AIR_CONDITIONING", "Aire Acondicionado");

        private static readonly IReadOnlyCollection<Amenity> _all = new List<Amenity>
        {
            Jacuzzi,
            Pool,
            Gym,
            Elevator,
            Garden,
            Terrace,
            AirConditioning
        }.AsReadOnly();

        public static IReadOnlyCollection<Amenity> All => _all;

        private Amenity(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public static Amenity FromCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new BussinesRuleException("El código de la comodidad es requerido.");
            }

            string normalizedCode = code.Replace(" ", "_")
                                        .ToUpper();

            Amenity? amenity = _all.FirstOrDefault(a => a.Code == normalizedCode);

            if (amenity == null)
            {
                throw new BussinesRuleException("No existe una comodidad con el código proporcionado.");
            }

            return amenity;
        }

        public bool Equals(Amenity? other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
