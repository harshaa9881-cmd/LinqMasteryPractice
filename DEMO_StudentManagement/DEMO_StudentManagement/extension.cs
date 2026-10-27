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

        public async Task Run()
        {


            await BasicAsync();

            //await TaskTrackingAsync();
            // await SequentialAsync();
            // await ConcurrentAsync();
            // await WhenAllAsync();
            // await WhenAnyAsync();
            // await ExceptionHandlingAsync();
            // await CancellationAsync();

        }
    }

}
