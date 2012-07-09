using System;
using System.Threading;
using Jurassic;

namespace Talifun.Javascript.Jurassic
{
	public class JurassicJavascriptRuntime : IJavascriptRuntime
	{
		protected readonly TimeSpan LockTimeout;
		protected readonly object ScriptEngineLock = new object();
		protected readonly Lazy<ScriptEngine> ScriptEngine;

		public JurassicJavascriptRuntime()
		{
			ScriptEngine = new Lazy<ScriptEngine>();
			LockTimeout = TimeSpan.FromSeconds(10);
		}

		public void LoadLibrary(string code)
		{
			if (!Monitor.TryEnter(ScriptEngineLock, LockTimeout))
			{
				throw new ApplicationException("Timeout waiting for lock - LoadLibrary");
			}
			try
			{
				ScriptEngine.Value.Execute(code);
			}
			finally
			{
				Monitor.Exit(ScriptEngineLock);
			}
		}

		public T ExecuteFunction<T>(string functionName, params object[] args)
		{
			T value;

			if (!Monitor.TryEnter(ScriptEngineLock, LockTimeout))
			{
				throw new ApplicationException("Timeout waiting for lock - ExecuteFunction");
			}
			try
			{
				value = ScriptEngine.Value.CallGlobalFunction<T>(functionName, args);
			}
			finally
			{
				Monitor.Exit(ScriptEngineLock);
			}

			return value;
		}

        #region IDisposable Members
        private int alreadyDisposed = 0;

		~JurassicJavascriptRuntime()
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
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposeManagedResources)
        {
            var disposedAlready = Interlocked.Exchange(ref alreadyDisposed, 1);
            if (disposedAlready != 0) return;

            if (!disposeManagedResources) return;

            // Dispose managed resources.
        }

        #endregion
	}
}
