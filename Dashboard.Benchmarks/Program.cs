using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.InProcess.Emit;
using Dashboard.Benchmarks;

var config = DefaultConfig.Instance
    .AddJob(Job.Default
        .WithToolchain(InProcessEmitToolchain.Instance)
        .WithWarmupCount(3)
        .WithIterationCount(5));

BenchmarkRunner.Run<BenchmarkService>(config);
