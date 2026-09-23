using Microsoft.EntityFrameworkCore;
using Properties.Domain.Entities.Properties;
using Properties.Persistence.Seed;

namespace Properties.Persistence.Seed.Properties
{
    public sealed class PropertyTypeSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public PropertyTypeSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 5;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.PropertyTypes.AnyAsync(cancellationToken))
            {
                return;
            }

            List<PropertyType> propertyTypes =
            [
                new PropertyType("Casa", "Vivienda unifamiliar independiente."),
                new PropertyType("Apartamento", "Unidad habitacional en edificio."),
                new PropertyType("Local", "Inmueble destinado a uso comercial."),
                new PropertyType("Oficina", "Espacio para uso administrativo o profesional."),
                new PropertyType("Lote", "Terreno sin construcción o con construcción mínima."),
                new PropertyType("Bodega", "Espacio para almacenamiento o logística."),
            ];

            await _context.PropertyTypes.AddRangeAsync(propertyTypes, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
