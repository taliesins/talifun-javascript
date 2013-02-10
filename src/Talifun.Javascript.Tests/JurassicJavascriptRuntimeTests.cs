using System;
using System.IO;
using System.Reflection;
using SubSpec;
using Talifun.Javascript.Jurassic;
using Xunit;

namespace Talifun.Javascript.Tests
{
    public class JurassicJavascriptRuntimeTests
    {
        [Specification]
        public void FunctionWithArgumentAndReturnResultTest()
        {
            IJavascriptRuntime javascriptRuntime = null;
            string javascriptReverseStringFunctionResult = string.Empty;
            @"Given a Jurassic Javascript Runtime
              And a reverse string javascript function".Context(() =>
                {
                    javascriptRuntime = new JurassicJavascriptRuntime();
                    using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.ReverseString.js")))
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

        //[Specification]
        //Fails
        public void BenchmarkResultTest()
        {
            IJavascriptRuntime javascriptRuntime = null;
            string javascriptBenchmarkFunctionResult = string.Empty;
            @"Given a Jurassic Javascript Runtime
              And a benchmark suite".Context(() =>
            {
                javascriptRuntime = new JurassicJavascriptRuntime();
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
                javascriptBenchmarkFunctionResult = javascriptRuntime.ExecuteFunction<string>("RunBenchMark", new object[] { });
            });

            @"Then benchmark results should be returned".Observation(() =>
            {
                Assert.NotNull(javascriptBenchmarkFunctionResult);
                Console.Write("Jurassic Benchmark - " + javascriptBenchmarkFunctionResult);
            });
        }

        [Specification]
        public void CoffeeScriptTest()
        {
            IJavascriptRuntime javascriptRuntime = null;
            string javascriptScriptFunctionResult = string.Empty;
            @"Given a Jurassic Javascript Runtime
              And coffee script compiler library".Context(() =>
            {
                javascriptRuntime = new JurassicJavascriptRuntime();
                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.CoffeeScript.js")))
                {
                    var coffeeScriptCompilerLibrary = reader.ReadToEnd();
                    javascriptRuntime.LoadLibrary(coffeeScriptCompilerLibrary);
                }
                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.RunCoffeeScriptCompiler.js")))
                {
                    var runCoffeeScriptCompilerLibrary = reader.ReadToEnd();
                    javascriptRuntime.LoadLibrary(runCoffeeScriptCompilerLibrary);
                }
            });

            @"When coffee script is compiled".Do(() =>
            {
                var coffeeScript = string.Empty;
                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.PlayerSpec.coffee")))
                {
                    coffeeScript = reader.ReadToEnd();
                }
                javascriptScriptFunctionResult = javascriptRuntime.ExecuteFunction<string>("RunCoffeeScriptCompiler", new object[] { coffeeScript });
            });

            @"Then javascript script should be returned".Observation(() =>
            {
                Assert.NotNull(javascriptScriptFunctionResult);
                Console.Write(javascriptScriptFunctionResult);
            });
        }
    }
}
