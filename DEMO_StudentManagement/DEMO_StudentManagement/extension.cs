using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        // ============================================
        // 3. SEQUENTIAL EXECUTION
        // ============================================

        public async Task SequentialAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();

            await DownloadFileAsync("File A", 2000);
            await DownloadFileAsync("File B", 3000);

            sw.Stop();

            Console.WriteLine(
                $"Sequential execution: {sw.ElapsedMilliseconds} ms");
        }
        // ============================================
        // 4. CONCURRENT EXECUTION
        // ============================================

        public async Task ConcurrentAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Task fileA = DownloadFileAsync("File A", 2000);
            Task fileB = DownloadFileAsync("File B", 3000);

            Console.WriteLine("Both operations have been started");

            await Task.WhenAll(fileA, fileB);

            sw.Stop();

            Console.WriteLine(
                $"Concurrent execution: {sw.ElapsedMilliseconds} ms");
        }

        private async Task DownloadFileAsync(string fileName, int delay)
        {

            Console.WriteLine($"{fileName}: Started");

            await Task.Delay(delay);

            Console.WriteLine($"{fileName}: Completed");
        }

        // ============================================
        // 5. TASK.WHENALL - MULTIPLE RESULTS
        // ============================================

        public async Task WhenAllAsync()
        {
            Task<int> task1 = GetValueAsync(2, 2000);
            Task<int> task2 = GetValueAsync(3, 3000);
            Task<int> task3 = GetValueAsync(4, 1000);

            //await GetValueAsync(2, 2000); // 2 seconds
            //await GetValueAsync(3, 3000); // 3 seconds
            //await GetValueAsync(4, 1000); // 1 second

            int[] results = await Task.WhenAll(
                task1, task2, task3);

            Console.WriteLine("All tasks completed");

            foreach (int result in results)
            {
                Console.WriteLine(result);
            }
        }

        private async Task<int> GetValueAsync(
            int value, int delay)
        {
            await Task.Delay(delay);
            return value * 10;
        }
        // ============================================
        // 6. TASK.WHENANY - FIRST TASK TO COMPLETE
        // ============================================

        public async Task WhenAnyAsync()
        {
            Task<string> task1 = GetMessageAsync(
                "Operation A", 4000);

            Task<string> task2 = GetMessageAsync(
                "Operation B", 1500);

            Task<string> task3 = GetMessageAsync(
                "Operation C", 3000);

            Task<string> completedTask = await Task.WhenAny(
                task1, task2, task3);

            Console.WriteLine(
                $"First completed: {await completedTask}");

            // Other tasks may still be running.
            await Task.WhenAll(task1, task2, task3);

            Console.WriteLine("All operations have now completed");
        }

        private async Task<string> GetMessageAsync(
            string message, int delay)
        {
            await Task.Delay(delay);
            return $"{message} finished";
        }
        // ============================================
        // 7. EXCEPTION HANDLING
        // ============================================

        public async Task ExceptionHandlingAsync()
        {
            try
            {
                Console.WriteLine("Starting operation...");

                await RiskyOperationAsync();

                Console.WriteLine("Operation succeeded");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Operation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Cleanup completed");
            }
        }

        private async Task RiskyOperationAsync()
        {
            await Task.Delay(1000);

            throw new InvalidOperationException(
                "Learner could not be registered");
        }
        // ============================================
        // 8. CANCELLATION TOKEN
        // ============================================

        public async Task CancellationAsync()
        {
            using CancellationTokenSource cts =
                new CancellationTokenSource();

            Task operation = ProcessLearnersAsync(cts.Token);

            // Demonstration: request cancellation after 2 seconds.
            await Task.Delay(2000);

            cts.Cancel();

            try
            {
                await operation;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled");
            }
        }

        private async Task ProcessLearnersAsync(
            CancellationToken token)
        {
            for (int i = 1; i <= 10; i++)
            {
                token.ThrowIfCancellationRequested();

                Console.WriteLine($"Processing learner {i}");

                await Task.Delay(1000, token);
            }

            Console.WriteLine("All learners processed");
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
