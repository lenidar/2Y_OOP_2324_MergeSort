using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Y_OOP_2324_MergeSort
{
    internal class Program
    {
        static Random rnd = new Random();
        static FileManager fm = new FileManager();
        static int[] numArr = new int[] { };
        static bool diagnosticsMode = true;
        static bool sortAsc = true;

        static async Task Main(string[] args)
        {
            //int[] numArr = new int[rnd.Next(5,20)];
            //int[] numArr = new int[7];
            //int[] numArr = new int[10000];

            System.Diagnostics.Stopwatch overall = new System.Diagnostics.Stopwatch();

            if (setup("data.csv"))
            {
                Console.WriteLine($"The size of the main array is {numArr.Length}");

                if (diagnosticsMode)
                {
                    overall.Start();
                    fm.massFileWriteAsync("UnsortedNumbers.txt", false, await convertToListString(numArr));
                }
                else
                {
                    Console.WriteLine("Unsorted Numbers...");
                    fm.massFileWriteAsync("UnsortedNumbers.txt", false, await convertToListString(numArr));
                    fm.fileWriteAsync("SortLog.txt", false, $"Algorithm start...");
                    displayArray(numArr);
                }

                numArr = arrSplitter(numArr, "0", 0, sortAsc, diagnosticsMode);

                if (diagnosticsMode)
                {
                    overall.Stop();
                    Console.WriteLine($"Algorithm took {overall.Elapsed} ms to sort {numArr.Length}");
                    fm.fileWriteAsync("SortLog.txt", true, $"Algorithm took {overall.Elapsed} ms to sort {numArr.Length}");
                    fm.massFileWriteAsync("SortedNumbers.txt", false, await convertToListString(numArr));
                }
                else
                {
                    Console.WriteLine("Sorted Numbers...");
                    fm.massFileWriteAsync("SortedNumbers.txt", false, await convertToListString(numArr));
                    displayArray(numArr);
                }
            }
            Console.ReadKey();
        }

        static int[] arrSplitter(int[] arr, string arrName, int nestCount, bool sortAsc, bool diagRun)
        {
            string pad = "";
            System.Diagnostics.Stopwatch mergeTimer = new System.Diagnostics.Stopwatch();
            mergeTimer.Start();

            for (int x = 0; x < nestCount; x++)
                pad += "  ";

            if(arr.Length > 2)
            {
                if(!diagRun)
                    Console.WriteLine($"{pad}Splitting {arrName} array to {arrName}-0 and {arrName}-1");

                int[] arrLeft = new int[(int)Math.Ceiling((float)arr.Length / 2)];
                int[] arrRight = new int[arr.Length - arrLeft.Length];

                arr = mergeTheSplit(arrSplitter(copyValues(arr, arrLeft, 0), arrName + "-0", nestCount + 1, sortAsc, diagRun)
                    , arrSplitter(copyValues(arr, arrRight, arrLeft.Length), arrName + "-1", nestCount + 1, sortAsc, diagRun));

                if (!diagRun)
                    Console.WriteLine($"{pad}Merged {arrName}-0 and {arrName}-1 back to {arrName}");
            }
            else 
            {
                if (!diagRun)
                    Console.WriteLine($"{pad}Unable to split array {arrName}");
            }

            //arr = selectionSort(arr);
            if (!diagRun)
                Console.WriteLine($"{pad}Sorting array {arrName}");
            arr = insertionSort(arr, sortAsc);

            mergeTimer.Stop();
            fm.fileWriteAsync("SortLog.txt", true
                , $"Algorithm took {mergeTimer.Elapsed} ms to sort {arrName} containing {arr.Length} items");
            return arr;
        }

        static int[] copyValues(int[] source, int[] target, int start)
        {
            for(int x = 0; x <target.Length; x++)
                target[x] = source[start + x];

            return target;
        }

        static int[] selectionSort(int[] source)
        {
            int moveTo = -1;
            int temp = 0;

            #region SelectionSort
            //for(int x = 0; x < source.Length - 1; x++)
            //{
            //    moveTo = -1;
            //    for (int y = x + 1; y < source.Length; y++)
            //    {
            //        if (source[x] > source[y])
            //        {
            //            moveTo = y;
            //            temp = source[x];
            //            break;
            //        }
            //    }

            //    if(moveTo > -1)
            //    {
            //        for(int y = x; y < moveTo; y++)
            //        {
            //            source[y] = source[y + 1];
            //        }
            //        source[moveTo] = temp;
            //        temp = -1;
            //    }
            //} 
            #endregion

            // bubble sort
            for (int a = 0; a < source.Length; a++)
            {
                for (int x = 0; x < source.Length - 1; x++)
                {
                    if (source[x] > source[x + 1])
                    {
                        temp = source[x + 1];
                        source[x + 1] = source[x];
                        source[x] = temp;
                    }
                }
            }

            return source;
        }

        static int[] insertionSort(int[] source, bool asc)
        {
            int temp = -1;

            for(int x = source.Length/2; x < source.Length; x++)
            {
                temp = source[x];
                for(int y = x - 1; y > -1; y--)
                {
                    if (temp < source[y] && asc)
                    {
                        source[y + 1] = source[y];
                        source[y] = temp;
                    }
                    else if (temp > source[y] && !asc)
                    {
                        source[y + 1] = source[y];
                        source[y] = temp;
                    }
                    else
                    {
                        break;
                    }
                }
            }


            return source;
        }

        static int[] mergeTheSplit(int[] left, int[] right)
        {
            int[] arr = new int[left.Length + right.Length];
            for (int x = 0; x < left.Length; x++)
                arr[x] = left[x];
            for (int x = 0; x < right.Length; x++)
                arr[x + left.Length] = right[x];

            return arr;
        }

        static void displayArray(int[] source)
        {
            int dCount = 1;
            foreach (int s in source)
            {
                Console.Write(s + "\t");
                if(dCount == 10)
                {
                    dCount = 0;
                    Console.WriteLine();
                }
                dCount++;
            }
            Console.WriteLine();
        }

        static bool setup(string path)
        {
            int[] array = new int[] { };

            List<string> content = fm.fileReader(path);

            bool cont = fm.processContent(content, out array);

            if(cont)
            {
                if (array[0] == 0)
                    diagnosticsMode = true;
                else
                    diagnosticsMode = false;

                if (array[1] == 0)
                    sortAsc = true;
                else
                    sortAsc = false;

                numArr = new int[array[2]];

                if (array.Length == 5)
                {
                    for(int x = 0; x < numArr.Length; x++)
                    {
                        numArr[x] = rnd.Next(array[3], array[4]);
                    }
                }
                else
                {
                    if (numArr.Length == array.Length - 3)
                    {
                        for(int x = 0; x < numArr.Length; x++)
                        {
                            numArr[x] = array[x + 3];
                        }
                    }
                    else
                        cont = false;
                }
            }

            return cont;
        }

        static async Task<List<string>> convertToListString(int[] source)
        {
            List<string> lines = new List<string>();

            foreach (int s in source)
                lines.Add(s + "");

            return lines;
        }
    }
}
