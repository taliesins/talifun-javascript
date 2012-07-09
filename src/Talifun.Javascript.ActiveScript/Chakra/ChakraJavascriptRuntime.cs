using Talifun.Javascript.ActiveScript;

namespace Talifun.Javascript.Chakra
{
	public class ChakraJavascriptRuntime : ActiveScriptJavascriptRuntimeBase
	{
		public ChakraJavascriptRuntime()
			: base(new ChakraJavaScriptEngine() as IActiveScript)
		{
		}
	}
}
