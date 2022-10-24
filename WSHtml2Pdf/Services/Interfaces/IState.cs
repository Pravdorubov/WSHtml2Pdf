namespace WSHtml2Pdf.Services.Interfaces
{
    public interface IState
    {
        bool IsConverting(string filename);
        bool IsHtmlLoaded(string htmlFilename);
        bool IsReadyToSend(string filename);
        bool IsSending(string pdfFilename);
        void SetReadyToSend(string filenamePdf, bool value);
        void StartConverting(string filename);
        void StopConverting(string filename);
        void StartSending(string filename);
        void StopSending(string filename);
    }
}
