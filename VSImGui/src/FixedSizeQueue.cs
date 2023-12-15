using System.Collections.Generic;

namespace VSImGui
{
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

        private int mLimit;

        public FixedSizedQueue(int limit) => Limit = limit;
        
        public void Enqueue(TElement obj)
        {
            Queue.Enqueue(obj);
            Shrink();
        }

        private void Shrink()
        {
            TElement overflow;
            while (Queue.Count > mLimit && Queue.TryDequeue(out overflow)) ;
        }
    }
}
