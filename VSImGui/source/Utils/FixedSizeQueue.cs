using System.Collections;

namespace VSImGui;

/// <summary>
/// Queue with fixed size. If size is exceeded extra elements are popped from beginning of the queue.<br/>
/// Meant to be used for plots.
/// </summary>
/// <typeparam name="TElement"></typeparam>
public class FixedSizedQueue<TElement> : IEnumerable<TElement>
{
    public FixedSizedQueue(int limit) => Limit = limit;

    /// <summary>
    /// Underlying queue
    /// </summary>
    public readonly Queue<TElement> Queue = new();
    /// <summary>
    /// Max number of elements. When set shrinks queue.
    /// </summary>
    public int Limit
    {
        get => _limit;

        set
        {
            _limit = value;
            Shrink();
        }
    }
    public int Count => Queue.Count;

    /// <summary>
    /// Add element to the end of the queue. Shrink queue if needed.
    /// </summary>
    /// <param name="obj"></param>
    public void Enqueue(TElement obj)
    {
        Queue.Enqueue(obj);
        Shrink();
    }

    public IEnumerator<TElement> GetEnumerator() => Queue.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Queue.GetEnumerator();
    public void Clear() => Queue.Clear();

    private int _limit;
    private void Shrink()
    {
        while (Queue.Count > _limit && Queue.TryDequeue(out _)) ;
    }
}
