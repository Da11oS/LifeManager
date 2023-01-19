namespace LM.Api;

public class Auths
{
    public string TokenKey { get; set; }
    
    public long AccessLifeTimeMs => 30 * 60_000;
    public long RefreshLifeTimeMs => (30 * 24 * 360_000);
}