using System.Diagnostics;

namespace GraphVizWrapper.Interfaces
{
    public interface IGetStartProcessQuery : IQuery<ProcessStartInfo, Process> { }
    public interface IGetProcessStartInfoQuery : IQuery<IProcessStartInfoWrapper, ProcessStartInfo> { }
}
