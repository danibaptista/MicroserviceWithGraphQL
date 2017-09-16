using System;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task CreateRequestForCommandAsync<T>(Guid id);

        Task<bool> ExistAsync(Guid id);
    }
}