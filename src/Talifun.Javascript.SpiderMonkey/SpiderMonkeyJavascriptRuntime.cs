using System;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using smnetjs;

namespace Talifun.Javascript.SpiderMonkey
{
    public class SpiderMonkeyJavascriptRuntime : IJavascriptRuntime
    {
        protected readonly TimeSpan LockTimeout;
        protected readonly object ScriptEngineLock = new object();
        protected readonly SMRuntime ScriptEngineContext;
        protected readonly SMScript ScriptEngine;
        protected readonly string ScriptName = "Global";

        public SpiderMonkeyJavascriptRuntime()
		{
            LockTimeout = TimeSpan.FromSeconds(10);
            ScriptEngineContext = new SMRuntime();
            ScriptEngine = ScriptEngineContext.InitScript(ScriptName);
		}

        public void LoadLibrary(string code)
        {
            ScriptEngine.Eval(code);
        }

        public T ExecuteFunction<T>(string functionName, params object[] args)
        {
            if (functionName == null)
                throw new ArgumentNullException("functionName");

            var arguments = GetArgs(args);
            var code = string.Format("{0}({1});", functionName, arguments);

            var result = ScriptEngine.Eval<T>(code);
            return result;
        }

        private string GetArgs(params object[] args)
        {
            if (args == null || args.Length < 1)
            {
                return string.Empty;
            }

            var argString = string.Join(", ", args.Select(JsonConvert.SerializeObject));

            return argString;
        }

        #region IDisposable Members
        private int alreadyDisposed = 0;

        ~SpiderMonkeyJavascriptRuntime()
        {
            // call Dispose with false.  Since we're in the
            // destructor call, the managed resources will be
            // disposed of anyways.
            Dispose(false);
        }

        void IDisposable.Dispose()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (alreadyDisposed != 0) return;

            // dispose of the managed and unmanaged resources
            Dispose(true);

            // tell the GC that the Finalize process no longer needs
            // to be run for this object. 

            // it is called after Dispose(true) to ensure that GC.SuppressFinalize() 
            // only gets called if the Dispose operation completes successfully. 
            //GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposeManagedResources)
        {
            var disposedAlready = Interlocked.Exchange(ref alreadyDisposed, 1);
            if (disposedAlready != 0) return;

            if (!disposeManagedResources) return;

            // Dispose managed resources.
            ScriptEngine.Dispose();
            ScriptEngineContext.Dispose();
        }

        #endregion
    }
}
