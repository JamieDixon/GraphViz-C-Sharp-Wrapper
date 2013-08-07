using System;
using System.Collections.Generic;
using System.IO;
using GraphVizWrapper.Interfaces;
using System.Reflection;

namespace GraphVizWrapper
{
    public class GraphVizWrapper : IGraphVizWrapper
    {
        private static Enums.RenderingEngine _renderingEngine;

        private static IGetStartProcessQuery _startProcessQuery;
        private static IGetProcessStartInfoQuery _getProcessStartInfoQuery;
        private static IRegisterLayoutPluginCommand _registerLayoutPlugincommand;

        private const string ProcessFolder = "GraphViz";
        private const string ConfigFile = "config6";

        // Public Properties

        public Enums.RenderingEngine RenderingEngine
        {
            get { return _renderingEngine; }
            set { _renderingEngine = value; }
        }

        // Private Properties

        private static string AssemblyDirectory
        {
            get
            {
                var uriBuilder = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
                string path = Uri.UnescapeDataString(uriBuilder.Path);
                return path.Substring(0, path.LastIndexOf('/'));
            }
        }


        private static string FilePath
        {
            get { return AssemblyDirectory + "/" + ProcessFolder + "/" + GetRenderingEngine(_renderingEngine) + ".exe"; }
        }

        private static bool ConfigExists
        {
            get { return File.Exists(AssemblyDirectory + "/" + ProcessFolder + "/" + ConfigFile); }
        }

        // Public Methods

        public GraphVizWrapper(IGetStartProcessQuery startProcessQuery, IGetProcessStartInfoQuery getProcessStartInfoQuery, IRegisterLayoutPluginCommand registerLayoutPlugincommand)
        {
            _startProcessQuery = startProcessQuery;
            _getProcessStartInfoQuery = getProcessStartInfoQuery;
            _registerLayoutPlugincommand = registerLayoutPlugincommand;
        }


        public byte[] GenerateGraph(string dotFile, Enums.GraphReturnType returnType)
        {
            byte[] output;

            if (!ConfigExists)
                _registerLayoutPlugincommand.Invoke();

            string fileType = GetReturnType(returnType);

            var processStartInfo = GetProcessStartInfo(fileType);

            using (var process = _startProcessQuery.Invoke(processStartInfo))
            {
                using (var stdIn = process.StandardInput)
                {
                    stdIn.WriteLine(dotFile);
                }

                using (var stdOut = process.StandardOutput)
                {
                    var baseStream = stdOut.BaseStream;
                    output = ReadFully(baseStream);
                }
            }

            return output;
        }

        // Private Methods

        private static System.Diagnostics.ProcessStartInfo GetProcessStartInfo(string returnType)
        {
            return _getProcessStartInfoQuery.Invoke(new ProcessStartInfoWrapper
            {
                FileName = FilePath,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                Arguments = "-v -o -T" + returnType,
                CreateNoWindow = true

            });
        }

        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private static string GetReturnType(Enums.GraphReturnType returnType)
        {
            var nameValues = new Dictionary<Enums.GraphReturnType, string>
                                 {
                                     {Enums.GraphReturnType.Png, "png"},
                                     {Enums.GraphReturnType.Jpg, "jpg"},
                                     {Enums.GraphReturnType.Pdf, "pdf"}
                                 };
            return nameValues[returnType];
        }

        private static string GetRenderingEngine(Enums.RenderingEngine renderingEngine)
        {
            var nameValues = new Dictionary<Enums.RenderingEngine, string>
                                 {
                                     {Enums.RenderingEngine.Dot, "dot"},
                                     {Enums.RenderingEngine.Fdp, "fdp"},
                                     {Enums.RenderingEngine.Neato, "neato"}
                                 };
            return nameValues[renderingEngine];
        }
    }
}
