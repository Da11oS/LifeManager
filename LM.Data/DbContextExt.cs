namespace LM.Data
{
    public partial class DbContext
    {
        public DbContext(DbContextOptions options) : base(options.PolyConnectionOptions)
        {
            InitMappingSchema();
        }

    }
}
