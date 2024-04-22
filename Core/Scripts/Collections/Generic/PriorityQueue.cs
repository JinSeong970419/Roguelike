using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.Core
{
    public class PriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>
    {
        public TElement First => throw new System.NotImplementedException();

        public int Count => throw new System.NotImplementedException();

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(TElement node)
        {
            throw new System.NotImplementedException();
        }

        public TElement Dequeue()
        {
            throw new System.NotImplementedException();
        }

        public void Enqueue(TElement node, TPriority priority)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        public void Remove(TElement node)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePriority(TElement node, TPriority priority)
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
