using Properties.Domain.Common.ValueObjects;
using Properties.Domain.Entities.Properties.ValueObjects;
using Properties.Domain.Exceptions;
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
        public Currency Price { get; private set; } = null!;
        public Guid PropertyTypeId { get; private set; }
        public PropertyType PropertyType { get; private set; } = null!;
        public Address Address { get; private set; } = null!;
        public bool IsAvailable { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
        public PropertyDetails Details { get; private set; } = null!;

        private readonly List<Amenity> _amenities = new();

        public List<Amenity> Amenities => _amenities;

        public Property(Guid ownerId, 
                        string title,
                        string description, 
                        Currency price, 
                        Guid propertyTypeId, 
                        Address address, bool isAvailable, 
                        PropertyDetails details)
        {
            ApplyOwnerIdRules(ownerId);
            ApplyTitleRules(title);
            ApplyDescriptionRules(description);
            ApplyPriceRules(price);
            ApplyDetailsRules(details);
            ApplyPropertyTypeRules(propertyTypeId);
            ApplyAddressRules(address);

            Id = Guid.CreateVersion7();
            OwnerId = ownerId;
            Title = title;
            Description = description;
            Price = price;
            PropertyTypeId = propertyTypeId;
            Address = address;
            IsAvailable = isAvailable;
            CreatedAt = DateTime.UtcNow;
            Details = details;
            isAvailable = true;
        }

        public void UpdateTitle(string title)
        {
            ApplyTitleRules(title);
            Title = title;
        }

        public void UpdateDescription(string description)
        {
            ApplyDescriptionRules(description);
            Description = description;
        }

        public void UpdatePrice(Currency price)
        {
            ApplyPriceRules(price);
            Price = price;
        }

        public void UpdateAddress(Address address)
        {
            ApplyAddressRules(address);
            Address = address;
        }

        public void MarkAsAvailable()
        {
            IsAvailable = true;
        }

        public void MarkAsUnavailable()
        {
            IsAvailable = false;
        }

        public void AddAmenity(Amenity amenity)
        {
            if (amenity is null)
            {
                throw new BussinesRuleException("La comodidad es requerida.");
            }

            if (_amenities.Any(a => a.Equals(amenity)))
            {
                throw new BussinesRuleException("La comodidad ya existe en la propiedad.");
            }

            _amenities.Add(amenity);
        }

        public void RemoveAmenity(Amenity amenity)
        {
            if (amenity is null)
            {
                throw new BussinesRuleException("La comodidad es requerida.");
            }

            Amenity? existingAmenity = _amenities.FirstOrDefault(a => a.Equals(amenity));

            if (existingAmenity is null)
            {
                throw new BussinesRuleException("La comodidad no existe en la propiedad.");
            }

            _amenities.Remove(amenity);
        }

        private void ApplyTitleRules(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BussinesRuleException("El título es requerido.");
            }

            if (title.Length < 4 || title.Length > 256)
            {
                throw new BussinesRuleException("El título debe tener entre 4 y 256 caracteres.");
            }
        }

        public void ApplyDescriptionRules(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new BussinesRuleException("La descripción es requerida.");
            }

            if (description.Length < 32)
            {
                throw new BussinesRuleException("La descripción debe tener al menos 32 caracteres.");
            }
        }

        private void ApplyPriceRules(Currency price)
        {
            if (price is null)
            {
                throw new BussinesRuleException("El precio es requerido.");
            }

            if (price.Amount <= 0)
            {
                throw new BussinesRuleException("El precio debe ser mayor a cero.");
            }
        }

        private void ApplyPropertyTypeRules(Guid propertyTypeId)
        {
            if (propertyTypeId == Guid.Empty)
            {
                throw new BussinesRuleException("El tipo de propiedad es requerido.");
            }
        }

        private void ApplyAddressRules(Address address)
        {
            if (address is null)
            {
                throw new BussinesRuleException("La dirección es requerida.");
            }
        }

        private void ApplyDetailsRules(PropertyDetails details)
        {
            if (details is null)
            {
                throw new BussinesRuleException("Los detalles de la propiedad son requeridos.");
            }
        }

        private void ApplyOwnerIdRules(Guid ownerId)
        {
            if (ownerId == Guid.Empty)
            {
                throw new BussinesRuleException("El propietario es requerido.");
            }
        }
    }
}
