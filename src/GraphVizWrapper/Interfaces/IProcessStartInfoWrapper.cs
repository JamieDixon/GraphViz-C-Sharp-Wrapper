namespace GraphVizWrapper.Interfaces
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
