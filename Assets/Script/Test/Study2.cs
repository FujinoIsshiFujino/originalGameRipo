using System.Collections;
using System.Collections.Generic;
using UnityEngine;




// sortの降順をsort を使わないで表現。foreachで繰り返しで大小比べて、一番大きい値を元の配列から削除して・・を繰り返す

// using System.Collections.Generic;
// using UnityEngine;

// public class DescendingSort : MonoBehaviour
// {
//     void Start()
//     {
//         List<int> numbers = new List<int>() { 5, 2, 9, 1, 3 };
//         List<int> sortedList = new List<int>();

//         while (numbers.Count > 0)
//         {
//             int max = FindMaxValue(numbers);
//             sortedList.Add(max);
//             numbers.Remove(max);
//         }

//         foreach (int number in sortedList)
//         {
//             Debug.Log(number);
//         }
//     }

//     int FindMaxValue(List<int> list)
//     {
//         int max = list[0];
//         foreach (int num in list)
//         {
//             if (num > max)
//             {
//                 max = num;
//             }
//         }
//         return max;
//     }
// }

