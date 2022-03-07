using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// see: https://www.geeksforgeeks.org/sorting-algorithms/

namespace Sorting
{

	public enum Algorithms {BubbleSort, CocktailSort, InsertionSort, ShellSort, BucketSort, QuickSort, QuickSortIterative}

	public class SortHelper
	{
		private Algorithms _currentAlgo;
		// private string typeName;
		// private const string methodName = "sort";
		// private Type type;
		// private MethodInfo method;

		public void SetAlgorithm(Algorithms algo)
		{
			_currentAlgo = algo;
			// typeName = "Sorting." + _currentAlgo.ToString();
			// type = Type.GetType(typeName);
			// if (type != null)
			// 	method = type.GetMethod(methodName);
			// else
			// 	method = null;
		}

		public Algorithms GetAlgorithm()
		{
			return _currentAlgo;
		}

		public void sort(Ball []arr)
		{
			// if (method != null)
			// 	method.Invoke(null, new object[]{arr});

			switch (_currentAlgo) {
				case Algorithms.BubbleSort:
					BubbleSort.sort(arr);
					break;
				case Algorithms.CocktailSort:
					CocktailSort.sort(arr);
					break;
				case Algorithms.InsertionSort:
					InsertionSort.sort(arr);
					break;
				case Algorithms.ShellSort:
					ShellSort.sort(arr);
					break;
				case Algorithms.BucketSort:
					BucketSort.sort(arr);
					break;
				case Algorithms.QuickSort:
					QuickSort.sort(arr);
					break;
				case Algorithms.QuickSortIterative:
					QuickSortIterative.sort(arr);
					break;
			}
		}

	}

	//----------------------------------
	//           Bubble Sort

    static class BubbleSort
    {
        public static void sort(Ball []arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
                for (int j = 0; j < n - i - 1; j++)
                    if (arr[j].Distance > arr[j + 1].Distance) {
                        // swap temp and arr[i]
                        Ball temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
        }
    }

	//----------------------------------
	//          Cocktail Sort

    static class CocktailSort
    {
        public static void sort(Ball []arr)
		{
			bool swapped = true;
			int start = 0;
			int end = arr.Length;

			while (swapped == true) {

				// reset the swapped flag on entering the loop, because it might be true from a previous iteration.
				swapped = false;

				// loop from bottom to top same as the bubble sort
				for (int i = start; i < end - 1; ++i) {
					if (arr[i].Distance > arr[i + 1].Distance) {
						Ball temp = arr[i];
						arr[i] = arr[i + 1];
						arr[i + 1] = temp;
						swapped = true;
					}
				}

				// if nothing moved, then array is sorted.
				if (swapped == false)
					break;

				// otherwise, reset the swapped flag so that it can be used in the next stage
				swapped = false;

				// move the end point back by one, because item at the end is in its rightful spot
				end = end - 1;

				// from top to bottom, doing the same comparison as in the previous stage
				for (int i = end - 1; i >= start; i--) {
					if (arr[i].Distance > arr[i + 1].Distance) {
						Ball temp = arr[i];
						arr[i] = arr[i + 1];
						arr[i + 1] = temp;
						swapped = true;
					}
				}

				// increase the starting point, because the last stage would have moved the next smallest number to its rightful spot.
				start = start + 1;
			}
		}
	}

  	//----------------------------------
	//         Insertion Sort

    static class InsertionSort
    {
        // Function to sort array using insertion sort
        public static void sort(Ball[] arr)
        {
            int n = arr.Length;
            for (int i = 1; i < n; ++i) {
                Ball key = arr[i];
                int j = i - 1;

                // Move elements of arr[0..i-1] that are greater than key to
                // one position ahead of their current position
                while (j >= 0 && arr[j].Distance > key.Distance) {
                    arr[j + 1] = arr[j];
                    j--;
                }
                arr[j + 1] = key;
            }
        }
    }

	//----------------------------------
	//           Shell Sort

    static class ShellSort
    {
        /* function to sort arr using shellSort */
        public static void sort(Ball []arr)
        {
            int n = arr.Length;

            // Start with a big gap, then reduce the gap
            for (int gap = n/2; gap > 0; gap /= 2)
            {
                // Do a gapped insertion sort for this gap size. The first gap elements a[0..gap-1] are already
                // in gapped order keep adding one more element until the entire array is gap sorted
                for (int i = gap; i < n; i += 1)
                {
                    // add a[i] to the elements that have been gap sorted save a[i] in temp and make a hole at position i
                    Ball temp = arr[i];

                    // shift earlier gap-sorted elements up until the correct location for a[i] is found
                    int j;
                    for (j = i; j >= gap && arr[j - gap].Distance > temp.Distance; j -= gap)
                        arr[j] = arr[j - gap];

                    // put temp (the original a[i])
                    // in its correct location
                    arr[j] = temp;
                }
            }
        }
    }

 	//----------------------------------
	//          Bucket Sort

    static class BucketSort
    {
        private static void isort(List<Ball> list)
        {
            for (int i = 1; i < list.Count; ++i)
            {
                Ball ball = list[i];   // object "ball" tobe sorted
                int j = i - 1;

                // while j is not zero, look for a position for v
                while (j >= 0 && list[j].Distance > ball.Distance)
                {
                    list[j + 1] = list[j];
                    j--;
                }
                // found a position
                list[j + 1] = ball;
            }
        }

        // Function to sort arr[] of size n using bucket sort
        private static void bucketSort(Ball []arr, int noOfBuckets)
        {
            if (noOfBuckets <= 0)
                return;

            // Find maximum and minimum element of the array
            float maxDistance = arr.Max(ball => ball.Distance);
            float minDistance = arr.Min(ball => ball.Distance);

            // Calculate the range of each bucket
            float range = (maxDistance - minDistance) / noOfBuckets;

            // Create noOfBuckets empty buckets
            List<Ball>[] buckets = new List<Ball>[noOfBuckets];
            for (int i = 0; i < noOfBuckets; i++) {
                buckets[i] = new List<Ball>();
            }

            // Scatter the array elements into the correct bucket
            for (int i = 0; i < arr.Count(); i++) {
                float floatIndex = (arr[i].Distance - minDistance) / range;
                // float diff = floatIndex - (int)floatIndex;

                // if (diff < 4.0E-06) {
                //     GD.Print(" Float Index: " + floatIndex);
                //     GD.Print("   Int Index: " + (int)floatIndex);
                //     GD.Print("        diff: " + diff);
                // }

                // append the boundary elements to the lower array
                //if (diff < 4.0E-06 && (arr[i].Distance != minDistance)) {
                if (floatIndex > noOfBuckets - 1) {
                    //GD.Print("     Index X: " + ((int)floatIndex - 1));
                    buckets[(int)floatIndex - 1].Add(arr[i]);
                }
                else {
                    buckets[(int)floatIndex].Add(arr[i]);
                }
            }

            // Sort each bucket individually
            for (int i = 0; i < noOfBuckets; i++) {
                if (buckets[i].Count() != 0) {
                    isort(buckets[i]);
                }
            }

            // Concatenate all buckets into arr[]
            int index = 0;
            for (int i = 0; i < noOfBuckets; i++) {
                for (int j = 0; j < buckets[i].Count(); j++) {
                    arr[index++] = buckets[i][j];
                }
            }
        }

        static public void sort(Ball[] arr)
	    {
    		bucketSort(arr, 25);
	    }
    }

    //----------------------------------
	//      Quick Sort recursive

    static class QuickSort
    {
        // This function takes last element as pivot, places the pivot element at its correct position in sorted
        // array, and places all smaller (smaller than pivot) to left of pivot and all greater elements to right of pivot
        private static int partition(Ball[] arr, int low, int high)
        {
            Ball temp;
            Ball pivot = arr[high];

            // index of smaller element
            int i = (low - 1);
            for (int j = low; j <= high - 1; j++) {

                // If current element is smaller than or equal to pivot
                if (arr[j].Distance <= pivot.Distance) {
                    i++;

                    // swap arr[i] and arr[j]
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            // swap arr[i+1] and arr[high] (or pivot)
            temp = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp;

            return i + 1;
        }

        // The main function that implements QuickSort()
        // arr[] --> Array to be sorted
        // low   --> Starting index
        // high  --> Ending index
        private static void qSort(Ball[] arr, int low, int high)
        {
            if (low < high) {
                // pi is partitioning index, arr[pi] is now at right place
                int pi = partition(arr, low, high);

                // Recursively sort elements before partition and after partition
                qSort(arr, low, pi - 1);
                qSort(arr, pi + 1, high);
            }
        }

        public static void sort(Ball[] arr)
	    {
    		qSort(arr, 0, arr.Length - 1);
	    }
    }

    //----------------------------------
	//      Quick Sort iterative

    static class QuickSortIterative
    {
        // This function takes last element as pivot, places the pivot element at its correct position in sorted array, and places all
        // smaller (smaller than pivot) to left of pivot and all greater elements to right of pivot

        private static int partition(Ball[] arr, int low, int high)
        {
            Ball temp;
            Ball pivot = arr[high];

            // index of smaller element
            int i = (low - 1);
            for (int j = low; j <= high - 1; j++) {
                // If current element is smaller than or equal to pivot
                if (arr[j].Distance <= pivot.Distance) {
                    i++;
                    // swap arr[i] and arr[j]
                    temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            // swap arr[i+1] and arr[high] (or pivot)
            temp = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp;

            return i + 1;
        }

        // arr[] --> Array to be sorted
        // low   --> Starting index
        // high  --> Ending index
        private static void qSortIterative(Ball[] arr, int low, int high)
        {
            // Create an auxiliary stack
            int[] stack = new int[high - low + 1];

            // initialize top of stack
            int top = -1;

            // push initial values of l and h to stack
            stack[++top] = low;
            stack[++top] = high;

            // Keep popping from stack while is not empty
            while (top >= 0) {
                // Pop h and l
                high = stack[top--];
                low = stack[top--];

                // Set pivot element at its correct position in sorted array
                int part = partition(arr, low, high);

                // If there are elements on left side of pivot, then push left side to stack
                if (part - 1 > low) {
                    stack[++top] = low;
                    stack[++top] = part - 1;
                }

                // If there are elements on right side of pivot, then push right side to stack
                if (part + 1 < high) {
                    stack[++top] = part + 1;
                    stack[++top] = high;
                }
            }
        }

        public static void sort(Ball[] arr)
	    {
    		qSortIterative(arr, 0, arr.Length - 1);
	    }
    }

    //----------------------------------
	// Insertion Sort - List Parameter

	class InsertionSortList {
		public void sort(List<Ball> list)
		{
			for (int i = 1; i < list.Count; ++i) {
				Ball ball = list[i];   // object "ball" tobe sorted
				int j = i - 1;

				// while j is not zero, look for a position for v
				while (j >= 0 && list[j].Distance > ball.Distance) {
					list[j + 1] = list[j];
					j--;
				}
				// found a position
				list[j + 1] = ball;
			}
		}
	}
}
