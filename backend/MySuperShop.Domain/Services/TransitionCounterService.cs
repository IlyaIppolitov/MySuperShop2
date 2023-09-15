using System.Collections.Concurrent;
using MySuperShop.Domain.Repositories;

namespace MySuperShop.Domain.Services;

public class TransitionCounterService : ITransitionCounterService
{
    private readonly ConcurrentDictionary<string, int> _counter = new ConcurrentDictionary<string, int>();

    public async Task ResetCounter()
    {
        _counter.Clear();
    }

    public async Task AddPath(string path)
    {
        if (_counter.ContainsKey(path))
        {
            ++_counter[path];
        }
        else
        {
            _counter.TryAdd(path, 1);
        }
    }

    public IDictionary<string, int> GetCounter() {
        return _counter;
    }
}