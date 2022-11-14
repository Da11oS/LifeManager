namespace LM.Data.RefreshKeys;

public interface IRefreshKeysRepository: IRepository<refresh_keys>
{
}

public class RefreshKeysRepository: Repository<refresh_keys>, IRefreshKeysRepository
{
    public RefreshKeysRepository(DbContext context) : base(context)
    {
    }

    
}