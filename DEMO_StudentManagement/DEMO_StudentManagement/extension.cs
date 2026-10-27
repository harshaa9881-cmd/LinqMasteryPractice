using System;
using System.Collections.Generic;
using System.Text;

namespace DEMO_StudentManagement
{
    public class LearnAsyncProgramming
    {
        // ============================================
        // 1. BASIC ASYNC / AWAIT
        // ============================================

        public async Task BasicAsync()
        {
            Console.WriteLine("Task 1 started");

            await Task.Delay(3000);

            Console.WriteLine("Task 1 completed");
            Console.WriteLine("Task 2 started");
        }

        // ============================================
        // 2. TASK<T> - RETURNING A VALUE
        // ============================================

        public async Task<int> TrackTaskAsync()
        {
            Console.WriteLine("Sub method started...");

            await Task.Delay(6000);

            Console.WriteLine("Sub method completed");

            return 12;
        }
        public async Task TaskTrackingAsync()
        {
            Console.WriteLine("Main method started");
            Task<int> task = TrackTaskAsync();
            Console.WriteLine("Back to main method");
            Console.WriteLine($"IsCompleted: {task.IsCompleted}");
            Console.WriteLine($"Status: {task.Status}");
            Console.WriteLine("Doing main method work...");
            int result = await task;
            Console.WriteLine($"Result: {result}");
            Console.WriteLine($"IsCompleted: {task.IsCompleted}");
            Console.WriteLine($"Status: {task.Status}");

            Console.WriteLine("Main method completed");
        }


        public async Task Run()
        {


            await BasicAsync();

            await TaskTrackingAsync();
            // await SequentialAsync();
            // await ConcurrentAsync();
            // await WhenAllAsync();
            // await WhenAnyAsync();
            // await ExceptionHandlingAsync();
            // await CancellationAsync();

        }
    }

}
