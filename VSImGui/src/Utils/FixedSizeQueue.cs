using System.Collections;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace VSImGui;

public class FixedSizedQueue<TElement>
{
    public readonly Queue<TElement> Queue = new();
    public int Limit {
        get => mLimit;

        set
        {
            mLimit = value;
            Shrink();
        }
    }
    public int Count => Queue.Count;
    
    public FixedSizedQueue(int limit) => Limit = limit;
    
    public void Enqueue(TElement obj)
    {
        Queue.Enqueue(obj);
        Shrink();
    }
    public TElement[] ToArray() => Queue.ToArray();
    public IEnumerable<TElement> Where(System.Func<TElement, bool> predicate) => Queue.Where(predicate);
    public IEnumerable<TResult> Select<TResult>(System.Func<TElement, int, TResult> selector) => Queue.Select(selector);
    public TElement Aggregate(System.Func<TElement, TElement, TElement> func) => Queue.Aggregate(func);

    private int mLimit;
    private void Shrink()
    {
        while (Queue.Count > mLimit && Queue.TryDequeue(out _)) ;
    }
}
