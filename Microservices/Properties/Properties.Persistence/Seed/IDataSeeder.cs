namespace Properties.Persistence.Seed
{
    /// <summary>
    /// Contrato de un seeder de infraestructura.
    /// Cada implementación carga un conjunto de datos iniciales (catálogos, datos demo, etc.).
    /// </summary>
    public interface IDataSeeder
    {
        /// <summary>
        /// Orden de ejecución. Menor = primero.
        /// Útil cuando un seeder depende de otro (Locations antes que Properties demo).
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Inserta datos si aún no existen. Debe ser idempotente.
        /// </summary>
        Task SeedAsync(CancellationToken cancellationToken = default);
    }
}
