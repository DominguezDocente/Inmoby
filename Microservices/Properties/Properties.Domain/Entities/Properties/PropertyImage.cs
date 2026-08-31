using Properties.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Domain.Entities.Properties
{
    public sealed class PropertyImage
    {
        public Guid Id { get; private set; }
        public Guid PropertyId { get; private set; }
        public Property Property { get; private set; }
        public string Url { get; private set; } = null!;
        public string? Description { get; private set; }
        public bool IsPrimary { get; private set; }
        public int DisplayOrder { get; private set; }

        public PropertyImage(Guid propertyId,
                             string url,
                             string? description,
                             bool isPrimary,
                             int displayOrder)
        {
            ApplyPropertyIdRules(propertyId);
            ApplyUrlRules(url);
            ApplyDescriptionRules(description);

            Id = Guid.CreateVersion7();
            PropertyId = propertyId;
            Url = url;
            Description = description;
            IsPrimary = isPrimary;
            DisplayOrder = displayOrder;
        }

        public void UpdateUrl(string url)
        {
            ApplyUrlRules(url);
            Url = url;
        }

        public void UpdateDescription(string? description)
        {
            ApplyDescriptionRules(description);
            Description = description;
        }

        public void UpdateDisplayOrder(int displayOrder)
        {
            DisplayOrder = displayOrder;
        }

        public void MarkAsPrimary()
        {
            IsPrimary = true;
        }
        public void UnmarkAsPrimary()
        {
            IsPrimary = false;
        }

        private void ApplyPropertyIdRules(Guid propertyId)
        {
            if (propertyId == Guid.Empty)
            {
                throw new BussinesRuleException("El Id de la propiedad es requerido.");
            }
        }

        private void ApplyUrlRules(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new BussinesRuleException("La URL de la imagen es requerida.");
            }

            if (url.Trim().Length < 8)
            {
                throw new BussinesRuleException("La URL de la imagen debe tener al menos 3 carácteres.");
            }

            if (url.Trim().Length >= 1024)
            {
                throw new BussinesRuleException("La URL de la imagen debe tener máximo 1024 carácteres.");
            }
        }

        private void ApplyDescriptionRules(string? description)
        {
            if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length >= 1024)
            {
                throw new BussinesRuleException("La descripción de la imagen debe tener máximo 1024 carácteres.");
            }
        }
    }
}
