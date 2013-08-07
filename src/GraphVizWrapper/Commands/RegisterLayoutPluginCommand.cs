using GraphVizWrapper.Interfaces;

namespace GraphVizWrapper.Commands
{
    public class RegisterLayoutPluginCommand : IRegisterLayoutPluginCommand
    {
        private readonly IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private readonly IGetStartProcessQuery _getStartProcessQuery;

        public RegisterLayoutPluginCommand(IGetProcessStartInfoQuery getProcessStartInfoQuery, IGetStartProcessQuery getStartProcessQuery)
        {
            _getStartProcessQuery = getStartProcessQuery;
            _getProcessStartInfoQuery = getProcessStartInfoQuery;
        }

        public void Invoke()
        {
            var processStartInfo = _getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
                                                 {
                                                     UseShellExecute = false,
                                                     Arguments = "-c",
                                                     CreateNoWindow = false
                                                 });

            using (_getStartProcessQuery.Invoke(processStartInfo)) { }
        }
    }
}
