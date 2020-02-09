public class Heap<T> where T : IHeapItem<T>
{
    public int Count { get; private set; }

    private T[] _items;

    public Heap(int maxHeapSize)
    {
        _items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = Count;

        _items[Count] = item;

        SortUp(item);

        Count++;
    }

    public T RemoveFirst()
    {
        T firstItem = _items[0];

        Count--;

        _items[0] = _items[Count];
        _items[0].HeapIndex = 0;

        SortDown(_items[0]);

        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
        SortDown(item);
    }

    public bool Contains(T item)
    {
        return Equals(_items[item.HeapIndex], item);
    }

    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = _items[parentIndex];

            if (item.CompareTo(parentItem) > 0)
                Swap(item, parentItem);
            else
                break;

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < Count)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < Count)
                    if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;

                if (item.CompareTo(_items[swapIndex]) < 0)
                    Swap(item, _items[swapIndex]);
                else
                    return;
            }
            else
                return;
        }
    }

    private void Swap(T firstItem, T secondItem)
    {
        _items[firstItem.HeapIndex] = secondItem;
        _items[secondItem.HeapIndex] = firstItem;

        int firstHeapIndex = firstItem.HeapIndex;

        firstItem.HeapIndex = secondItem.HeapIndex;
        secondItem.HeapIndex = firstHeapIndex;
    }
}