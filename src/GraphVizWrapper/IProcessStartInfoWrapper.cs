// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProcessStartInfoWrapper.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the IProcessStartInfoWrapper interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace GraphVizWrapper
{
    public interface IProcessStartInfoWrapper
    {
        string FileName { get; set; }
        string Arguments { get; set; }

        bool RedirectStandardInput { get; set; }
        bool RedirectStandardOutput { get; set; }
        bool RedirectStandardError { get; set; }
        bool UseShellExecute { get; set; }
        bool CreateNoWindow { get; set; }
    }
}
