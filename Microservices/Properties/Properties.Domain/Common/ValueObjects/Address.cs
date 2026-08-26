using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Common.ValueObjects
{
    public sealed class Address
    {
        // Via Principal Ej: Calle 4a South 
        public RoadTypeEnum? MainRoadType { get; private set; }
        public string MainRoadNumber { get; private set; } = null!;
        public string? MainRoadLetter { get; private set; }
        public RoadSuffixEnum? MainRoadSuffix { get; private set; }

        // Vía que cruza Ej: # 76b
        public string CrossRoadNumber { get; set; }
        public string? CrossRoadLetter { get; private set; }
        public RoadSuffixEnum? CrossRoadSuffix { get; private set; }

        // Placa de predio Ej: 101
        public string? Plate { get; private set; }

        public string? Indications { get; set; }
    }
}
