using System.Collections.Generic;

namespace DebuggingTools
{
    public interface IDebuggableObject
    {
        public Dictionary<string, string> GetDebugValues();
    }
}
