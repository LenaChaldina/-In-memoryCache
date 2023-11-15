# In-memory Cache
Created a generic in-memory cache component based on LRU Cache.
https://en.wikipedia.org/wiki/Cache_replacement_policies#LRU

To simplify the implementation, ready-made data structures were taken: Dictionary and LinkedList.

Used lock() to make components thread-safe for all methods. To improve the program, it would be better to use ReaderWriterLock.
https://learn.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?view=net-7.0&redirectedfrom=MSDN

Created the OnItemEvicted event which allows the consumer to know when items get evicted.

