using System.Collections.Concurrent;

namespace MySuperShop.Domain.Repositories;

public interface ITransitionCounterService
{
    Task ResetCounter();
    Task AddPath(string path);
    IDictionary<string, int> GetCounter();
}