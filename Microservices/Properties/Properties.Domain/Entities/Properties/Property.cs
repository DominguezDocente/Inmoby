using Properties.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Properties
{
    public sealed class Property
    {
        public Guid Id { get; private set; }
        public Guid OwnerId { get; set; }
        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public Currency  Price { get; private set; } = null!;
        public Guid PropertyTypeId { get; private set; }
        public PropertyType PropertyType { get; set; } = null!;


    }
}
