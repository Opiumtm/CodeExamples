using System;
using System.Globalization;
using NativeComponent;

namespace ReflectionBenchmark.Algorithms
{
    /// <summary>
    /// C# bubble sort.
    /// </summary>
    public sealed class DotNetBubbleSortRunner : IBenchmarkRunner
    {
        private int[] _data;

        public void Run(int count)
        {
            for (var i = 0; i < count; i++)
            {
                DoSort();
            }
        }

        private void DoSort()
        {
            var array = new int[4096];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i % 100;
            }
            BubbleSort(array);
            _data = array;
        }

        private void BubbleSort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        Swap(array, j, j+1);
                    }
                }
            }
        }

        private void Swap(int[] array, int idx1, int idx2)
        {
            int tmp = array[idx1];
            array[idx1] = array[idx2];
            array[idx2] = tmp;
        }
    }
}