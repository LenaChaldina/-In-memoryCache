namespace MemoryCache;

public abstract class Program
{
    private static void Main()
    {
        var cache = new LruCache<string, int>(3);

        cache.ItemEvicted += (sender, key) => { Console.WriteLine($"Evicted: {key}"); };

        cache.Add("one", 1);
        cache.Add("two", 2);
        cache.Add("three", 3);

        Console.WriteLine("Cache content:");
        PrintCacheContent(cache);

        // Access an existing item (moves it to the front of the LRU list)
        var value = cache.Get("two");
        Console.WriteLine($"Value for key 'two': {value}");

        // Add a new item (evicts the least recently used item)
        cache.Add("four", 4);

        Console.WriteLine("Updated cache content:");
        PrintCacheContent(cache);
    }

    private static void PrintCacheContent(LruCache<string, int> cache)
    {
        foreach (var node in cache)
        {
            Console.WriteLine($"Cached Value: {node}");
        }

        Console.WriteLine();
    }
}