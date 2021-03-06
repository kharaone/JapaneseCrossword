﻿using System.Collections.Generic;
using System.Timers;

namespace General
{
    public static class ComplexColectionHelpers
    {
        public static int[,] ListArrayToJaggedArrayVertical(List<int>[] listArray)
        {
            var maxCount = GetBiggestListCount(listArray);
            var jaggedArray = new int[listArray.Length, maxCount];
            for (var row = 0; row < listArray.Length; row++)
            {
                var columns = listArray[row];
                for (var col = 0; col < columns.Count; col++)
                {
                    jaggedArray[row, col] = columns[col];
                }
            }

            return jaggedArray;
        }

        public static int[,] ListArrayToJaggedArrayHorizontal(List<int>[] listArray)
        {
            var maxCount = GetBiggestListCount(listArray);
            var jaggedArray = new int[maxCount, listArray.Length];
            for (var row = 0; row < listArray.Length; row++)
            {
                var columns = listArray[row];
                for (var col = 0; col < columns.Count; col++)
                {
                    jaggedArray[col, row] = columns[col];
                }
            }

            return jaggedArray;
        }

        private static int GetBiggestListCount(List<int>[] arrayList)
        {
            var maxCount = 0;
            foreach (var list in arrayList)
            {
                if (list.Count > maxCount)
                    maxCount = list.Count;
            }

            var timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Stop();
            
            return maxCount;
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public static int[,] InvertOrientation(this int[,] array)
        {
            var output = new int[array.GetLength(1), array.GetLength(0)];
            for (var row = 0; row < array.GetLength(0); row++)
            {
                for (var col = 0; col < array.GetLength(1); col++)
                {
                    output[col, row] = array[row, col];
                }
            }

            return output;
        }
    }
}
