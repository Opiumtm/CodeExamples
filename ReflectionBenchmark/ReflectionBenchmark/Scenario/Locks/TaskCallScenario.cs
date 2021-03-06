﻿using System;
using System.Threading;
using System.Threading.Tasks;
using ReflectionBenchmark.Callable;
using ReflectionBenchmark.DynamicCall;

namespace ReflectionBenchmark.Locks
{
    /// <summary>
    /// Task call
    /// </summary>
    public class TaskCallScenario : IScenario
    {
        public string ScenarioName => "Task tcs call";

        public async Task<BenchmarkResult> DoBenchmark()
        {
            var task = Task.Factory.StartNew(RunBenchmark);
            return await task;
        }

        private BenchmarkResult RunBenchmark()
        {
            var callable = new CallableClass();
            var ticks1 = Environment.TickCount;
            for (var i = 0; i < Consts.RunCount * 100; i++)
            {
                var tcs = new TaskCompletionSource<bool>();
                callable.Run();
                callable.RunWithArgs(i, "");
                tcs.SetResult(true);
                tcs.Task.Wait();
            }
            var ticks2 = Environment.TickCount;
            return new BenchmarkResult() { RunCount = Consts.RunCount * 10, Milliseconds = ticks2 - ticks1 };
        }
    }
}