using System.Data;

namespace WebAPI.Services
{
    public interface IConnectionService
    {
        IDbConnection EstablishConnection();
    }
}