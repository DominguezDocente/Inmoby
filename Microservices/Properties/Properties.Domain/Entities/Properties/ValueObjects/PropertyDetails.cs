using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Properties.ValueObjects
{
    public sealed record PropertyDetails
    {
        public int Bedrooms { get; private set; }
        public int ParkingSpaces { get; private set; }
        public int BathRooms { get; private set; }
        public int Stratum { get; private set; }
        public double Area { get; private set; }
        public PropertiCondition PropertiCondition { get; private set; }

        public PropertyDetails(int bedrooms, int parkingSpaces, int bathRooms, int stratum, double area, PropertiCondition propertiCondition)
        {
            ApplyAreaRules(area);
            ApplyBedroomsRules(bedrooms);
            ApplyBathroomsRules(bathRooms);
            ApplyStratumRules(stratum);
            ApplyParkingSpacesRules(parkingSpaces);
            ApplyConditionRules(propertiCondition);

            Bedrooms = bedrooms;
            ParkingSpaces = parkingSpaces;
            BathRooms = bathRooms;
            Stratum = stratum;
            Area = area;
            PropertiCondition = propertiCondition;
        }

        private void ApplyBedroomsRules(int bedrooms)
        {
            if (bedrooms <= 0)
            {
                throw new BussinesRuleException("El inmueble debe contener al menos una habitación");
            }
        }

        private void ApplyBathroomsRules(int bathrooms)
        {
            if (bathrooms <= 0)
            {
                throw new BussinesRuleException("El inmueble debe contener al menos un baño");
            }
        }

        private void ApplyParkingSpacesRules(int parkingSpaces)
        {
            if (parkingSpaces < 0)
            {
                throw new BussinesRuleException("El inmueble debe contener al menos un espacio de parqueo");
            }
        }

        private void ApplyStratumRules(int stratum)
        {
            if (stratum < 1 || stratum > 6)
            {
                throw new BussinesRuleException("El estrato debe estar entre 1 y 6");
            }
        }

        private void ApplyAreaRules(double area)
        {
            if (area <= 20)
            {
                throw new BussinesRuleException("El inmueble debe contener al menos 20 metros cuadrados");
            }
        }

        private void ApplyConditionRules(PropertiCondition condition)
        {
            if (!Enum.IsDefined(condition))
            {
                throw new BussinesRuleException("La condición del inmueble es inválida");
            }
        }
    }
}
