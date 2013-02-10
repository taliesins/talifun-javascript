namespace Talifun.Javascript
{
    public interface IJavascriptRuntimeGlobalVariable
    {
        T GetVariable<T>(string variableName);
        void SetVariable(string variableName, object value);
    }
}