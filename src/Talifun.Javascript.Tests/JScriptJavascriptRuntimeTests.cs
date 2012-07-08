using SubSpec;
using Talifun.Javascript.JScript;
using Xunit;

namespace Talifun.Javascript.Tests
{
    public class JScriptJavascriptRuntimeTests
    {
        public const string JavascriptReverseStringFunction = @"
function ReverseString(stringToReverse)
{
    return stringToReverse.split('').reverse().join('');
}

";

        [Specification]
        public void FunctionWithArgumentAndReturnResultTest()
        {
            IJavascriptRuntime javascriptRuntime = null;
            string javascriptReverseStringFunctionResult = string.Empty;
            @"Given a V8 Javascript Runtime
              And a reverse string javascript function".Context(() =>
                {
                    javascriptRuntime = new JScriptJavascriptRuntime();
                    javascriptRuntime.LoadLibrary(JavascriptReverseStringFunction);
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
    }
}
