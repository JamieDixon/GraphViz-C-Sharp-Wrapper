// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetProcessStartInfoQuery.cs" company="Jamie Dixon Ltd">
//   Jamie Dixon
// </copyright>
// <summary>
//   Defines the GetProcessStartInfoQuery type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;

namespace GraphVizWrapper.Queries
{
    public class GetProcessStartInfoQuery : IGetProcessStartInfoQuery
    {
        public System.Diagnostics.ProcessStartInfo Invoke(IProcessStartInfoWrapper startInfoWrapper)
        {
            return new System.Diagnostics.ProcessStartInfo
                       {
                           WorkingDirectory = Path.GetDirectoryName(startInfoWrapper.FileName) ?? "",
                           FileName = '"' + startInfoWrapper.FileName + '"',
                           Arguments = startInfoWrapper.Arguments,
                           RedirectStandardInput = startInfoWrapper.RedirectStandardInput,
                           RedirectStandardOutput = startInfoWrapper.RedirectStandardOutput,
                           RedirectStandardError = startInfoWrapper.RedirectStandardError,
                           UseShellExecute = startInfoWrapper.UseShellExecute,
                           CreateNoWindow = startInfoWrapper.CreateNoWindow
                       };
        }
    }
}
