using System;
using System.IO;
using System.Reflection;
using SubSpec;
using Talifun.Javascript.V8;
using Xunit;

namespace Talifun.Javascript.Tests
{
    public class V8JavascriptRuntimeTests
    {
        [Specification]
        public void FunctionWithArgumentAndReturnResultTest()
        {
            IJavascriptRuntime javascriptRuntime = null;
            string javascriptReverseStringFunctionResult = string.Empty;
            @"Given a V8 Javascript Runtime
              And a reverse string javascript function".Context(() =>
                {
                    javascriptRuntime = new V8JavascriptRuntime();

                    using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.Benchmark.js")))
                    {
                        var reverseStringLibrary = reader.ReadToEnd();
                        javascriptRuntime.LoadLibrary(reverseStringLibrary);
                    }
                });

            @"When reverse string javascript function is called with 'test' as argument".Do(() =>
                {
                    javascriptReverseStringFunctionResult = javascriptRuntime.ExecuteFunction<string>("ReverseString", "test");
                });

            @"Then 'tset' should be returned".Observation(() =>
                {
                    Assert.Equal("tset", javascriptReverseStringFunctionResult);
                });

        }

        [Specification]
        public void BenchmarkResultTest()
        {
            IJavascriptRuntime javascriptRuntime = null;
            string javascriptBenchmarkFunctionResult = string.Empty;
            @"Given a V8 Javascript Runtime
              And a benchmark suite".Context(() =>
            {
                javascriptRuntime = new V8JavascriptRuntime();
                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.Benchmark.js")))
                {
                    var benchmarkLibrary = reader.ReadToEnd();
                    javascriptRuntime.LoadLibrary(benchmarkLibrary);
                }
                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.RunBenchmark.js")))
                {
                    var runbenchmarkLibrary = reader.ReadToEnd();
                    javascriptRuntime.LoadLibrary(runbenchmarkLibrary);
                }
            });

            @"When benchmark suite is run".Do(() =>
            {
                javascriptBenchmarkFunctionResult = javascriptRuntime.ExecuteFunction<string>("RunBenchMark", new object[]{});
            });

            @"Then benchmark results should be returned".Observation(() =>
            {
                Assert.NotNull(javascriptBenchmarkFunctionResult);
                Console.Write(javascriptBenchmarkFunctionResult);
            });
        }

        [Specification]
        public void CoffeeScriptTest()
        {
            IJavascriptRuntime javascriptRuntime = null;
            string javascriptBenchmarkFunctionResult = string.Empty;
            @"Given a V8 Javascript Runtime
              And coffee script compiler library".Context(() =>
            {
                javascriptRuntime = new V8JavascriptRuntime();
                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.CoffeeScript.js")))
                {
                    var coffeeScriptCompilerLibrary = reader.ReadToEnd();
                    javascriptRuntime.LoadLibrary(coffeeScriptCompilerLibrary);
                }
            });

            @"When benchmark suite is run".Do(() =>
            {
                javascriptBenchmarkFunctionResult = javascriptRuntime.ExecuteFunction<string>("RunBenchMark", new object[] { });
            });

            @"Then benchmark results should be returned".Observation(() =>
            {
                Assert.NotNull(javascriptBenchmarkFunctionResult);
                Console.Write(javascriptBenchmarkFunctionResult);
            });
        }

    }
}
