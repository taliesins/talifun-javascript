using Talifun.Javascript.ActiveScript;

namespace Talifun.Javascript.JScript
{
	public class JScriptJavascriptRuntime : ActiveScriptJavascriptRuntimeBase
	{
		public JScriptJavascriptRuntime()
			: base(new JScriptJavaScriptEngine() as IActiveScript)
		{
		}
	}
}
