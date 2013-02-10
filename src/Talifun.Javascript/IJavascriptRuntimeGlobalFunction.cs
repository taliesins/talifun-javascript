using System;

namespace Talifun.Javascript
{
    public interface IJavascriptRuntimeGlobalFunction
    {
        void SetFunction(string functionName, Delegate function);
    }
}