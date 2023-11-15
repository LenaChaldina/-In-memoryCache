using System.Collections;

namespace MemoryCache;

public class LruCache<TKey, TValue> : IEnumerable<TKey> where TKey : notnull
{
    private readonly object _lruListLock = new object();
    private readonly Dictionary<TKey, TValue> _cacheDictionary;
    private readonly LinkedList<TKey> _lruList;
    private readonly int _maxCapacity;

    public event EventHandler<TKey>? ItemEvicted;

    public LruCache(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than zero.");
        }

        this._maxCapacity = capacity;
        this._cacheDictionary = new Dictionary<TKey, TValue>();
        this._lruList = new LinkedList<TKey>();
    }

    public TValue? Get(TKey key)
    {
        if (!_cacheDictionary.ContainsKey(key)) return default(TValue);
        // Move the accessed item to the front of the LRU list
        lock (_lruListLock)
        {
            _lruList.Remove(key);
            _lruList.AddFirst(key);
            return _cacheDictionary[key];
        }
    }

    public void Add(TKey key, TValue value)
    {
        if (_cacheDictionary.Count >= _maxCapacity)
        {
            Evict();
        }

        lock (_lruListLock)
        {
            _lruList.AddFirst(key);
            _cacheDictionary.TryAdd(key, value);
        }
    }

    private void Evict()
    {
        LinkedListNode<TKey>? lastNode;
        lock (_lruListLock)
        {
            lastNode = _lruList.Last;
            if (lastNode != null)
            {
                _cacheDictionary.Remove(lastNode.Value, out _);
                _lruList.RemoveLast();
            }
        }

        if (lastNode != null) OnItemEvicted(lastNode.Value);
    }

    private void OnItemEvicted(TKey item)
    {
        ItemEvicted?.Invoke(this, item);
    }

    public IEnumerator<TKey> GetEnumerator()
    {
        LinkedListNode<TKey>? currentNode, nextNode;
        lock (_lruListLock)
        {
            currentNode = _lruList.First;
            nextNode = currentNode?.Next;
        }

        while (currentNode != null)
        {
            yield return currentNode.Value;
            lock (_lruListLock)
            {
                currentNode = nextNode;
                nextNode = currentNode?.Next;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}