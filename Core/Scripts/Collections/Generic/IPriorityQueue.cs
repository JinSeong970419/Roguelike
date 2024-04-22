using System.Collections.Generic;

namespace Roguelike.Core
{
    public interface IPriorityQueue<TElement, TPriority> : IEnumerable<TElement>
    {
        void Enqueue(TElement element, TPriority priority);
        TElement Dequeue();
        void Clear();
        bool Contains(TElement element);
        void Remove(TElement element);
        void UpdatePriority(TElement element, TPriority priority);
        TElement First { get; }
        int Count { get; }
    }
}
