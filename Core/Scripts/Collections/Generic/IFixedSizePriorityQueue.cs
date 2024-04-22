namespace Roguelike.Core
{
    internal interface IFixedSizePriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>
    {
        void Resize(int maxNodes);
        int MaxSize { get; }
        void ResetNode(TElement node);
    }
}
