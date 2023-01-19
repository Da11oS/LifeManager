using DataModel;

namespace LM.Data.RefreshKeys;

public interface IRefreshKeysRepository: IRepository<AdmSchema.RefreshKey>
{
}

public class RefreshKeysRepository: Repository<AdmSchema.RefreshKey>, IRefreshKeysRepository
{
    public RefreshKeysRepository(LifeManagerDb context) : base(context)
    {
    }
}