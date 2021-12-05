using System.Collections.Concurrent;

namespace Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ForEachTests
{
    private List<Guid> _guids = new ();
    private List<Guid> _selected = new ();
    private ConcurrentBag<Guid> _selectedBag = new ();
    private ConcurrentBag<Guid> _selectedBag2 = new ();
    private int _responseTime = 10;
    
    public ForEachTests()
    {
        for (var i = 0; i < 1000; i++)
        {
            _guids.Add(Guid.NewGuid());
        }
    }

    private void _addToSelected(Guid guid, int delay)
    {
        Task.Delay(delay);
        _selected.Add(guid);
    }

    private void _addToBag(Guid guid, int delay)
    {
        Task.Delay(delay);
        _selectedBag.Add(guid);
    }

    private async Task _addToBagAsync(Guid guid, int delay)
    {
        await Task.Delay(delay);
        _selectedBag2.Add(guid);
    }

    [Benchmark]
    public int ForEachNormal()
    {
        foreach (var guid in _guids)
        {
            if (guid.ToString().Contains("42"))
            {
                _addToSelected(guid, 0);
            }
        }
        return _selected.Count;
    }
  
    [Benchmark]
    public int ParallelForEach()
    {
        Parallel.ForEach(_guids,  (item) =>
        {
            if (item.ToString().Contains("42"))
            {
                _addToBag(item, 0);
            }
        });
        return _selectedBag.Count;
    }
    
    [Benchmark]
    public async Task<int> ParallelForEachAsync()
    {
        await Parallel.ForEachAsync(_guids, async (item, cancellationToken) =>
        {
            if (item.ToString().Contains("42"))
            {
                await _addToBagAsync(item, 0);
            }
        });
        return _selectedBag2.Count;
    }
    
    [Benchmark]
    public int ForEachNormalWithResponseTime()
    {
        foreach (var guid in _guids)
        {
            if (guid.ToString().Contains("42"))
            {
                _addToSelected(guid, _responseTime);
            }
        }
        return _selected.Count;
    }
  
    [Benchmark]
    public int ParallelForEachWithResponseTime()
    {
        Parallel.ForEach(_guids,  (item) =>
        {
            if (item.ToString().Contains("42"))
            {
                _addToBag(item, _responseTime);
            }
        });
        return _selectedBag.Count;
    }
    
    [Benchmark]
    public async Task<int> ParallelForEachAsyncWithResponseTime()
    {
        await Parallel.ForEachAsync(_guids, async (item, cancellationToken) =>
        {
            if (item.ToString().Contains("42"))
            {
                await _addToBagAsync(item, _responseTime);
            }
        });
        return _selectedBag2.Count;
    }


}