using System.Collections.Generic;

namespace DebuggingTools
{
    /// <summary>
    /// Classes can implement this interface to send debug values to the debugging tool
    /// </summary>
    public interface IDebuggableObject
    {
        /// <summary>
        /// Sends debug values to the debugging tool
        /// </summary>
        /// <returns>A dictionary of all values to be displayed in the debugging tool</returns>
        public Dictionary<string, string> GetDebugValues();
    }
}
