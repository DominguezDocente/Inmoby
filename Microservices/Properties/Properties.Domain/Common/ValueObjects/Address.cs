using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Common.ValueObjects
{
    public sealed class Address
    {
        // Via Principal Ej: Calle 4a South 
        public RoadTypeEnum MainRoadType { get; private set; }
        public string MainRoadNumber { get; private set; } = null!;
        public string? MainRoadLetter { get; private set; }
        public RoadSuffixEnum? MainRoadSuffix { get; private set; }

        // Vía que cruza Ej: # 76b
        public string CrossRoadNumber { get; set; }
        public string? CrossRoadLetter { get; private set; }
        public RoadSuffixEnum? CrossRoadSuffix { get; private set; }

        public Guid CountryId { get; private set; }
        public Guid StateId { get; private set; }
        public Guid CityId { get; private set; }
        public Guid NeighborhoodId { get; private set; }

        // Placa de predio Ej: 101
        public string? Plate { get; private set; }

        public string? Indications { get; set; }

        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public Address(RoadTypeEnum mainRoadType,
                       string mainRoadNumber,
                       string? mainRoadLetter,
                       RoadSuffixEnum? mainRoadSuffix,
                       string crossRoadNumber,
                       string? crossRoadLetter,
                       RoadSuffixEnum? crossRoadSuffix,
                       Guid countryId,
                       Guid stateId,
                       Guid cityId,
                       Guid neighborhoodId,
                       string? plate,
                       string? indications,
                       string? postalCode,
                       double? latitude,
                       double? longitude)
        {   
            ApplyRoadTypeRules(mainRoadType);
            ApplyRoadNumberRules(mainRoadNumber, 16);
            ApplyRoadLetterRules(mainRoadLetter);
            ApplyRoadSuffixRules(mainRoadSuffix);

            ApplyRoadNumberRules(crossRoadNumber, 16);
            ApplyRoadLetterRules(crossRoadLetter);
            ApplyRoadSuffixRules(crossRoadSuffix);

            ApplyPlateNumberRules(plate);
            ApplyPostalCodeRules(postalCode);
            ApplyCoordinatesRules(latitude, longitude);

            MainRoadType = mainRoadType;
            MainRoadNumber = mainRoadNumber;
            MainRoadLetter = mainRoadLetter;
            MainRoadSuffix = mainRoadSuffix;
            CrossRoadNumber = crossRoadNumber;
            CrossRoadLetter = crossRoadLetter;
            CrossRoadSuffix = crossRoadSuffix;
            CountryId = countryId;
            StateId = stateId;
            CityId = cityId;
            NeighborhoodId = neighborhoodId;
            Plate = plate;
            Indications = indications;
            PostalCode = postalCode;
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            string main = FormatRoadSegment(MainRoadNumber, MainRoadLetter, MainRoadSuffix);
            string cross = FormatRoadSegment(CrossRoadNumber, CrossRoadLetter, CrossRoadSuffix);
            return $"{MainRoadType} {main} # {cross} - {Plate}";
        }

        private static string FormatRoadSegment(string number, string? letter, RoadSuffixEnum? suffix)
        {
            StringBuilder sb = new StringBuilder(number);

            if (letter is not null) 
            {
                sb.Append(letter);
            }

            if (suffix is not null)
            {
                sb.Append(ToSpanishSuffix(suffix));
            }

            return sb.ToString();
        }

        private static string ToSpanishSuffix(RoadSuffixEnum? suffix) => suffix switch
        {
            RoadSuffixEnum.North => "Norte",
            RoadSuffixEnum.South => "Sur",
            RoadSuffixEnum.East => "Este",
            RoadSuffixEnum.West => "Oeste",
            _ => string.Empty
        };

        private static void ApplyRoadTypeRules(RoadTypeEnum roadType)
        {
            if (!Enum.IsDefined(roadType))
            {
                throw new BussinesRuleException("El tipo de vía no es válido");
            }
        }

        private static void ApplyRoadNumberRules(string number, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                throw new BussinesRuleException("El número de vía es requerido");
            }

            if (number.Length > maxLength)
            {
                throw new BussinesRuleException($"El número de vía no puede exceder {maxLength} caracteres");
            }
        }

        private static void ApplyRoadLetterRules(string? letter)
        {
            if (!string.IsNullOrEmpty(letter) && letter.Length > 4)
            {
                throw new BussinesRuleException($"La letra de vía no puede exceder 4 caracteres");
            }
        }

        private static void ApplyRoadSuffixRules(RoadSuffixEnum? roadSuffix)
        {
            if (roadSuffix is not null && !Enum.IsDefined((RoadSuffixEnum)roadSuffix))
            {
                throw new BussinesRuleException("El sufijo de vía no es válido");
            }
        }

        private static void ApplyPlateNumberRules(string? number)
        {
            if (!string.IsNullOrEmpty(number) && number.Length > 16)
            {
                throw new BussinesRuleException("El número de placa no puede exceder 16 caracteres");
            }
        }

        private static void ApplyPostalCodeRules(string? number)
        {
            if (!string.IsNullOrEmpty(number) && number.Length > 8)
            {
                throw new BussinesRuleException("El código postal no puede exceder 8 caracteres");
            }
        }

        private static void ApplyCoordinatesRules(double? latitude, double? longitude)
        {
            if (latitude is null && longitude is null)
            {
                return;
            }

            if (latitude is null || longitude is null)
            {
                throw new BussinesRuleException("La Latitud y Longitud deben enviarse juntas.");
            }

            if (latitude < -90 || latitude > 90)
            {
                throw new BussinesRuleException("La latitud debe estar entre -90 y 90 grados");
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new BussinesRuleException("La longitud debe estar entre -180 y 180 grados");
            }
        }
    }
}
