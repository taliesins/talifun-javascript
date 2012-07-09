using System;
using System.IO;
using System.Reflection;
using SubSpec;
using Talifun.Javascript.Chakra;
using Xunit;

namespace Talifun.Javascript.Tests
{
    public class ChakraJavascriptRuntimeTests
    {
        [Specification]
        public void FunctionWithArgumentAndReturnResultTest()
        {
             const string javascriptReverseStringFunction = @"
function ReverseString(stringToReverse)
{
    return stringToReverse.split('').reverse().join('');
}
";

            IJavascriptRuntime javascriptRuntime = null;
            string javascriptReverseStringFunctionResult = string.Empty;
            @"Given a Chakra Javascript Runtime
              And a reverse string javascript function".Context(() =>
                {
                    javascriptRuntime = new ChakraJavascriptRuntime();
                    javascriptRuntime.LoadLibrary(javascriptReverseStringFunction);
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
            @"Given a Chakra Javascript Runtime
              And a benchmark suite".Context(() =>
            {
                javascriptRuntime = new ChakraJavascriptRuntime();
                using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.Benchmark.js")))
                {
                    var benchmarkLibrary = reader.ReadToEnd();
                    javascriptRuntime.LoadLibrary(benchmarkLibrary);
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
