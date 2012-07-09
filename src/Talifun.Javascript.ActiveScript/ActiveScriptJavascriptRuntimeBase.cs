using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Talifun.Javascript.ActiveScript
{
	public abstract class ActiveScriptJavascriptRuntimeBase : BaseActiveScriptSite, IJavascriptRuntime
	{
		protected IActiveScript ScriptEngine;
		protected IActiveScriptParseWrapper JsParse;
		protected object JsDispatch;
		protected Type JsDispatchType;

		protected Dictionary<string, object> SiteItems = new Dictionary<string, object>();

		protected ActiveScriptJavascriptRuntimeBase(IActiveScript scriptEngine)
		{
			ScriptEngine = scriptEngine;
			ScriptEngine.SetScriptSite(this);
			JsParse = new ActiveScriptParseWrapper(ScriptEngine);
			JsParse.InitNew();
		}

		public void LoadLibrary(string libraryCode)
		{
			try
			{
				JsParse.ParseScriptText(libraryCode, null, null, null, IntPtr.Zero, 0, ScriptTextFlags.IsVisible);
			}
			catch
			{
				var last = GetAndResetLastException();
				if (last != null)
					throw last;
				else throw;
			}
			// Check for parse error
			var parseError = GetAndResetLastException();
			if (parseError != null)
				throw parseError;

			UpdateDispatch();
		}

		public T ExecuteFunction<T>(string functionName, params object[] args)
		{
			T result;
			try
			{
				result = (T)JsDispatchType.InvokeMember(functionName, BindingFlags.InvokeMethod, null, JsDispatch, args);
			}
			catch
			{
				ThrowError();
				throw;
			}

			ThrowError();

			// TODO: This is a hack, but I'm not sure how else to test for invalid statements
			//if (result == "this;")
			//    throw new ArgumentException(string.Format("{0}('{1}'); is not valid JavaScript.", function, input));

			return result;
		}

		~ActiveScriptJavascriptRuntimeBase()
		{
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing) {
            ComRelease(ref JsDispatch, !disposing);

            // For now these next two actually reference the same object, but it doesn't hurt to be explicit.
            ComRelease(ref JsParse, !disposing);

			ComRelease(ref ScriptEngine, !disposing);
        }

		private void UpdateDispatch()
		{
			ComRelease(ref JsDispatch);
			ScriptEngine.GetScriptDispatch(null, out JsDispatch);
			JsDispatchType = JsDispatch.GetType();
		}

        private void ComRelease<T>(ref T o, bool final = false)
            where T : class {
            if (o != null && Marshal.IsComObject(o)) {
                if (final)
                    Marshal.FinalReleaseComObject(o);
                else
                    Marshal.ReleaseComObject(o);
            }
            o = null;
        }

		private void ThrowError()
		{
			var last = GetAndResetLastException();
			if (last != null)
				throw last;
		}

		public override object GetItem(string name)
		{
			lock (SiteItems)
			{
				object result = null;
				return SiteItems.TryGetValue(name, out result) ? result : null;
			}
		}

		public override IntPtr GetTypeInfo(string name)
		{
			lock (SiteItems)
			{
				return !SiteItems.ContainsKey(name) ? IntPtr.Zero : Marshal.GetITypeInfoForType(SiteItems[name].GetType());
			}
		}
	}
}
