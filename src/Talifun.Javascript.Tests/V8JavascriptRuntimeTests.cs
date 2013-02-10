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
        public void GettingGlobalVariableTest()
        {
            var javascriptRuntime = default(V8JavascriptRuntime);
            var javascriptGlobalVariable = string.Empty;

            @"Given a V8 Javascript Runtime
              And a global variable".Context(() =>
                {
                    javascriptRuntime = new V8JavascriptRuntime();

                    using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.ReverseString.js")))
                    {
                        var reverseStringLibrary = reader.ReadToEnd();
                        javascriptRuntime.LoadLibrary(reverseStringLibrary);
                    }
                });

            @"When getting global variable".Do(() =>
            {
                javascriptGlobalVariable = javascriptRuntime.GetVariable<string>("test");
            });

            @"Then 'tset' should be returned".Observation(() =>
            {
                Assert.Equal("tset", javascriptGlobalVariable);
            });
        }

        [Specification]
        public void SettingGlobalVariableTest()
        {
            var javascriptRuntime = default(V8JavascriptRuntime);
            var javascriptReverseStringFunctionResult = string.Empty;

            @"Given a V8 Javascript Runtime
              And a reverse string delegate function
              And a global variable to reverse".Context(() =>
                {
                    javascriptRuntime = new V8JavascriptRuntime();

                    using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("Talifun.Javascript.Tests.scripts.ReverseString.js")))
                    {
                        var reverseStringLibrary = reader.ReadToEnd();
                        javascriptRuntime.LoadLibrary(reverseStringLibrary);
                    }

                    javascriptRuntime.SetVariable("stringToReverse", "test");
                });

            @"When reverse string javascript function is called that uses global variable".Do(() =>
            {
                javascriptReverseStringFunctionResult = javascriptRuntime.ExecuteFunction<string>("ReverseString");
            });

            @"Then 'tset' should be returned".Observation(() =>
            {
                Assert.Equal("tset", javascriptReverseStringFunctionResult);
            });
        }


        [Specification]
        public void UsingGlobalFunctionTest()
        {
            var javascriptRuntime = default(V8JavascriptRuntime);
            var javascriptReverseStringFunctionResult = string.Empty;
            @"Given a V8 Javascript Runtime
              And a reverse string delegate function".Context(() =>
                {
                    javascriptRuntime = new V8JavascriptRuntime();

                    Func<string, string> reverseStringMethod = delegate(string stringToReverse)
                        {
                            var stringToReverseArray = stringToReverse.ToCharArray();
                            Array.Reverse(stringToReverseArray);
                            return new string(stringToReverseArray);
                        };

                    javascriptRuntime.SetFunction("ReverseString", reverseStringMethod);
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
        public void FunctionWithArgumentAndReturnResultTest()
        {
            var javascriptRuntime = default(V8JavascriptRuntime);
            var javascriptReverseStringFunctionResult = string.Empty;
            @"Given a V8 Javascript Runtime
              And a reverse string javascript function".Context(() =>
                {
                    javascriptRuntime = new V8JavascriptRuntime();

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

        [Specification]
        public void BenchmarkResultTest()
        {
            var javascriptRuntime = default(V8JavascriptRuntime);
            var javascriptBenchmarkFunctionResult = string.Empty;
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
                Console.Write("V8 Benchmark - " + javascriptBenchmarkFunctionResult);
            });
        }

        [Specification]
        public void CoffeeScriptTest()
        {
            var javascriptRuntime = default(V8JavascriptRuntime);
            var javascriptScriptFunctionResult = string.Empty;
            @"Given a V8 Javascript Runtime
              And coffee script compiler library".Context(() =>
            {
                javascriptRuntime = new V8JavascriptRuntime();
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
