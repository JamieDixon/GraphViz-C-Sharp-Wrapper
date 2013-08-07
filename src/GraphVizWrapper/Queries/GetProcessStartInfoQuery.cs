using GraphVizWrapper.Interfaces;

namespace GraphVizWrapper.Queries
{
    public class GetProcessStartInfoQuery : IGetProcessStartInfoQuery
    {
        public System.Diagnostics.ProcessStartInfo Invoke(IProcessStartInfoWrapper startInfoWrapper)
        {
            return new System.Diagnostics.ProcessStartInfo
                       {
                           FileName = startInfoWrapper.FileName,
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
