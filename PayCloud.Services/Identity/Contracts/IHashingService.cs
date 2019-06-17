namespace PayCloud.Services.Identity.Contracts
{
    public interface IHashingService
    {
        string GetHashedString(string inputString);
    }
}