using GraphVizWrapper.Interfaces;

namespace GraphVizWrapper
{
    public class ProcessStartInfoWrapper : IProcessStartInfoWrapper
    {
        public string FileName { get; set; }
        public string Arguments { get; set; }

        public bool RedirectStandardInput { get; set; }
        public bool RedirectStandardOutput { get; set; }
        public bool RedirectStandardError { get; set; }
        public bool UseShellExecute { get; set; }
        public bool CreateNoWindow { get; set; }
    }
}
