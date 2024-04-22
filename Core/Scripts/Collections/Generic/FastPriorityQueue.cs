using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelike.Core
{
    public sealed class FastPriorityQueue<TElement> : IPriorityQueue<TElement, float> where TElement : class
    {
        private struct Node
        {
            public float Priority { get; set; }
            public int Index { get; set; }
            public TElement Element { get; set; }
        }

        private Node[] _nodes;
        private int _count;

        public TElement First => throw new System.NotImplementedException();

        public int Count => _count;
        public int Capacity { get { return _nodes.Length; } }

        public FastPriorityQueue(int capacity)
        {
            _nodes = new Node[capacity];
            _count = 0;
        }

        public void Enqueue(TElement element, float priority)
        {
            Node node = new Node();
            node.Priority = priority;
            node.Element = element;

            _nodes[_count] = node;
            node.Index = _count;
            _count++;
        }

        public TElement Dequeue()
        {
            throw new System.NotImplementedException();
        }

        public void Remove(TElement node)
        {
            throw new System.NotImplementedException();
        }

        public void UpdatePriority(TElement node, float priority)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            Array.Clear(_nodes, 0, _count);
            _count = 0;
        }

        public bool Contains(TElement node)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _nodes[i].Element;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
