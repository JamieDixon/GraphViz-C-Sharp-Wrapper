using System.Diagnostics;
using GraphVizWrapper.Interfaces;

namespace GraphVizWrapper.Queries
{
    public class GetStartProcessQuery : IGetStartProcessQuery
    {
        public Process Invoke(ProcessStartInfo processStartInfo)
        {
            return Process.Start(processStartInfo);
        }
    }
}