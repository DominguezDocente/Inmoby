using Properties.Domain.Entities.Locations;
using Microsoft.EntityFrameworkCore;
using Properties.Domain.Entities.Properties;

namespace Properties.Persistence.Seeds.Properties
{
    public class PropertyTypeSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public PropertyTypeSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.PropertyTypes.AnyAsync(cancellationToken))
            {
                return;
            }

            List<PropertyType> types =
            [
                new PropertyType("Casa", "Vivienda independiente"),
                new PropertyType("Apartamento", "Unidad habitacional en edificio"),
                new PropertyType("Local", "Inmueble destinado a uso comercial"),
                new PropertyType("Oficina", "Espacio para uso administrativo o profdesional"),
                new PropertyType("Lote", "Terreno sin o con poca construcción"),
                new PropertyType("Bodega", "Espacio para almacenamiento logístico"),
            ];

            await _context.PropertyTypes.AddRangeAsync(types, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
