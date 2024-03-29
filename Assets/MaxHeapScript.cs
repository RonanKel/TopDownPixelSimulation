using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maxheap
{
    public class MaxHeap
    {
        private List<GameObject> heap;
        private Dictionary<GameObject, int> indexMap;

        public MaxHeap()
        {
            heap = new List<GameObject>();
            indexMap = new Dictionary<GameObject, int>();
        }

        public int Count => heap.Count;

        public void Add(GameObject gameObject)
        {
            heap.Add(gameObject);
            int currentIndex = heap.Count - 1;
            indexMap[gameObject] = currentIndex;
            HeapifyUp(currentIndex);
        }

        public GameObject ExtractMax()
        {
            if (heap.Count == 0)
                throw new System.InvalidOperationException("Heap is empty.");

            GameObject max = heap[0];
            Swap(0, heap.Count - 1);
            indexMap.Remove(max);
            heap.RemoveAt(heap.Count - 1);
            HeapifyDown(0);

            return max;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (Compare(heap[index], heap[parentIndex]) > 0)
                {
                    Swap(index, parentIndex);
                    index = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private void HeapifyDown(int index)
        {
            int maxIndex = index;
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;

            if (leftChildIndex < heap.Count && Compare(heap[leftChildIndex], heap[maxIndex]) > 0)
                maxIndex = leftChildIndex;
            if (rightChildIndex < heap.Count && Compare(heap[rightChildIndex], heap[maxIndex]) > 0)
                maxIndex = rightChildIndex;

            if (maxIndex != index)
            {
                Swap(index, maxIndex);
                HeapifyDown(maxIndex);
            }
        }

        private void Swap(int i, int j)
        {
            GameObject temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
            indexMap[heap[i]] = i;
            indexMap[heap[j]] = j;
        }

        private int Compare(GameObject obj1, GameObject obj2)
        {
            CellScript cellScript1 = obj1.GetComponent<CellScript>();
            CellScript cellScript2 = obj2.GetComponent<CellScript>();

            if (cellScript1 == null || cellScript2 == null)
                throw new System.InvalidOperationException("One or both objects do not have CellScript component.");

            return cellScript1.waterLevel.CompareTo(cellScript2.waterLevel);
        }
    }
}