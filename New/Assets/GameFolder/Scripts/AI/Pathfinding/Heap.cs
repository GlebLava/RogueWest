using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    T[] heapItems;
    int currentHeapItemCount;

    public Heap(int maxHeapSize)
    {
        heapItems = new T[maxHeapSize];
    }

    public void Add(T heapItem)
    {
        heapItem.HeapIndex = currentHeapItemCount;
        heapItems[currentHeapItemCount] = heapItem;
        SortUp(heapItem);
        currentHeapItemCount++;
    }

    public T RemoveFirst()
    {
        T firstHeapItem = heapItems[0];
        currentHeapItemCount--;
        heapItems[0] = heapItems[currentHeapItemCount];
        heapItems[0].HeapIndex = 0;
        SortDown(heapItems[0]);
        return firstHeapItem;
    }
    public void UpdateItem(T heapItem)
    {
        SortUp(heapItem);
    }

    public int Count
    {
        get
        {
            return currentHeapItemCount;
        }
    }

    public bool Contains(T heapItem)
    {
        return Equals(heapItems[heapItem.HeapIndex], heapItem);
    }

    void SortDown(T heapItem)
    {
        while (true)
        {
            int childIndexLeft = heapItem.HeapIndex * 2 + 1;
            int childIndexRight = heapItem.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < currentHeapItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < currentHeapItemCount)
                {
                    if (heapItems[childIndexLeft].CompareTo(heapItems[childIndexRight]) < 0)
                        swapIndex = childIndexRight;

                }

                if (heapItem.CompareTo(heapItems[swapIndex]) < 0)
                    Swap(heapItem, heapItems[swapIndex]);
                else
                    return;
            }
            else
                return;
        }
    }

    void SortUp(T heapItem)
    {
        int parentIndex = (heapItem.HeapIndex - 1) / 2;

        while (true)
        {
            T parentHeapItem = heapItems[parentIndex];
            if (heapItem.CompareTo(parentHeapItem) > 0)
            {
                Swap(heapItem, parentHeapItem);
            }
            else
            {
                break;
            }

            parentIndex = (heapItem.HeapIndex - 1) / 2;
        }
    }

    void Swap(T heapItemA, T heapItemB)
    {
        heapItems[heapItemA.HeapIndex] = heapItemB;
        heapItems[heapItemB.HeapIndex] = heapItemA;
        int heapItemAIndex = heapItemA.HeapIndex;
        heapItemA.HeapIndex = heapItemB.HeapIndex;
        heapItemB.HeapIndex = heapItemAIndex;

    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
