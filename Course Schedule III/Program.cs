using System;
using System.Collections.Generic;
using System.Linq;

namespace Course_Schedule_III
{
  class Program
  {
    static void Main(string[] args)
    {
      int[][] courses = new int[7][]{ new int[] { 10, 20 } , new int[] { 4, 18 } , new int[] { 5, 20 } ,
        new int[] { 9, 10 } , new int[] { 3, 12 } , new int[] { 10, 19 }, new int[] { 7, 17 } };

      Solution s = new Solution();
      var answer = s.ScheduleCourse(courses);
      Console.WriteLine(answer);
    }
  }
  public class Solution
  {
    public class MaxPQ : IComparer<int>
    {
      public int Compare(int a, int b)
      {
        if (b > a) return 1;
        else if (b == a) return 0;
        else return -1;
      }
    }
    public int ScheduleCourse(int[][] courses)
    {
      // Step 1
      // Courses has duration and lastday to complete the course
      // We are sorting the input array on lastday, so that
      // again sorting it on duration, in case two courses are having same lastday but the duration is different , in that case we want the lesser duration course to be appear before the other, ex - [(5, 10), (3, 10)] => [(3, 10), (5, 10)]

      // Step 1
      var coursesList = courses.ToList();
      var sorted = coursesList.OrderBy(x => x[1]).ThenBy(x => x[0]);
      // priority queue will have the max duration courses on the top, so while traversing the course array we find current course can not be completed within current provided time, then will try to swap with the max duration course which already we have taken, but only if the current course duration is lesser than the max duration from PQ. 
      PriorityQueue<int, int> maxHeap = new PriorityQueue<int, int>(new MaxPQ());
      int totalTime = 0;
      foreach (var item in sorted)
      {
        int duration = item[0];
        int lastDay = item[1];
        // calculate the new total time req when taken current course
        int newTotalTime = totalTime + duration;
        if (newTotalTime <= lastDay)
        {
          // if we can complete this course within current timeline will add to PQ 
          totalTime += duration;
          maxHeap.Enqueue(lastDay, duration);
        }
        else
        {
          // When taking the current course can not be completed within current timeline
          // look for the maximum course duration element from PQ which already we taken 
          maxHeap.TryPeek(out int maxLastDay, out int maxDuration);
          // if the current duration to complete the current course is lesser than the max duration course which we have taken already, why ? if the current couse is greater than the max couse element from PQ then taking current element will not solve our problem to complete the course coz we are still maximizing our competion time, instead we should try to minimize the completion time
          if (duration < maxDuration)
          {
            // lets say current course duration is lesser, we need to remove the max course from our total time and add the current course duration in total comlpetion time
            int tempTotalTime = totalTime;
            totalTime -= maxDuration;
            totalTime += duration;
            if (totalTime <= lastDay)
            {
              // if after removing and adding the current course duration we are still able to complete current course within given timeline will do following
              // will remove the already taken max course
              maxHeap.Dequeue();
              // will add the current course
              maxHeap.Enqueue(lastDay, duration);
              // We have swapped the max course with the current course, as our job to complete as much as course 
            }
            else
            {
              // in case taking the current course still we are unable to complete it within given current timeline
              // reset the totalTime and skip taking the current course
              totalTime = tempTotalTime;
            }
          }
        }
      }
      return maxHeap.Count;
    }
  }
}
