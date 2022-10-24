using WSHtml2Pdf.Services.Interfaces;

namespace WSHtml2Pdf.Services.Implementations
{
    public class State : IState
    {
        private IWebHostEnvironment environment;
        private IConfiguration configuration;

        private HashSet<string> ConvertingFiles = new HashSet<string>();
        private HashSet<string> SendingFiles = new HashSet<string>();
        private HashSet<string> PdfsReadyToSend = new HashSet<string>();

        public State(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.environment = environment;
            this.configuration = configuration;
        }

        public bool IsConverting(string filename)
        {
            return ConvertingFiles.Contains(filename);
        }

        public void StartConverting(string filename)
        {
            ConvertingFiles.Add(filename);
        }

        public void StopConverting(string filename)
        {
            ConvertingFiles.Remove(filename);
        }

        public bool IsReadyToSend(string filename)
        {
            return PdfsReadyToSend.Contains(filename);
        }

        public void SetReadyToSend(string filenamePdf, bool value)
        {
            if (value)
            {
                PdfsReadyToSend.Add(filenamePdf);
                return;
            }

            PdfsReadyToSend.Remove(filenamePdf);
        }

        public bool IsHtmlLoaded(string htmlFilename)
        {
            string path = Path.Combine(environment.WebRootPath, configuration["htmlfilesdir"], htmlFilename);
            return File.Exists(path);
        }

        public bool IsSending(string filename)
        {
            return SendingFiles.Contains(filename);
        }

        public void StartSending(string filename)
        {
            SendingFiles.Add(filename);
        }

        public void StopSending(string filename)
        {
            SendingFiles.Remove(filename);
        }
    }
}
