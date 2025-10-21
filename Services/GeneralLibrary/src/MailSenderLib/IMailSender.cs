namespace MailSenderLib
{
    public interface IMailSender
    {
        Task SendMail(string senderDisplayName, string senderEmail, string subject, string body, string recipientAddress, string? receipintDisplayName = null);
        Task SendMailWtAttachement(string senderDisplayName, string senderEmail, string subject, string body, string recipientEmails, string? receipintDisplayName = null, params (byte[] content, string fileName)[] attachments);
        Task SendRichHtmlMail(string senderDisplayName, string senderEmail, string subject, string htmlBody, string recipientEmails, string? receipintDisplayName = null, string? plainTextFallback = null);
        Task SendMailWithHtmlEmbedded(string senderDisplayName, string senderEmail, string subject, string plainTextBody, string htmlContent, string recipientEmails, string? receipintDisplayName = null);
        Task SendMailWithHtmlEmbeddedAndAttached(string senderDisplayName, string senderEmail, string subject, string plainTextBody, string htmlContent, string recipientEmails, string? receipintDisplayName = null, string? attachmentFileName = null);
    }
}