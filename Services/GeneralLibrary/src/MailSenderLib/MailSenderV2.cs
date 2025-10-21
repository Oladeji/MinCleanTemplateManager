using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MailSenderLib;

/// <summary>
/// Enhanced email sender with improved architecture, separation of concerns, and testability
/// </summary>
public class MailSenderV2 : IMailSender
{
    private readonly ILogger<MailSenderV2> _logger;
    private readonly EmailConfiguration _emailConfig;
    private readonly ISmtpClientFactory _smtpClientFactory;

    public MailSenderV2(ILogger<MailSenderV2> logger, IConfiguration configuration, ISmtpClientFactory? smtpClientFactory = null)
    {
        _logger = logger;
        _smtpClientFactory = smtpClientFactory ?? new SmtpClientFactory();
        _emailConfig = LoadEmailConfiguration(configuration);
    }

    /// <summary>
    /// Sends a basic email (delegates to SendRichHtmlMail for backward compatibility)
    /// </summary>
    public Task SendMail(string senderDisplayName, string senderEmail, string subject, string body, string recipientEmails, string? receipintDisplayName = null)
    {
        return SendRichHtmlMail(senderDisplayName, senderEmail, subject, body, recipientEmails, receipintDisplayName);
    }

    /// <summary>
    /// Enhanced HTML email sender with better MIME support and fallback text
    /// </summary>
    public async Task SendRichHtmlMail(string senderDisplayName, string senderEmail, string subject, string htmlBody, string recipientEmails, string? receipintDisplayName = null, string? plainTextFallback = null)
    {
        var emailMessage = new EmailMessage
        {
            SenderDisplayName = senderDisplayName,
            SenderEmail = senderEmail,

            Subject = subject,
            HtmlBody = htmlBody,
            PlainTextBody = plainTextFallback,
            RecipientEmails = ParseRecipientEmails(recipientEmails),
            RecipientDisplayName = receipintDisplayName
        };

        await SendEmailAsync(emailMessage, EmailComposer.ComposeRichHtmlEmail);
    }

    /// <summary>
    /// Sends an email with both plain text and HTML content embedded
    /// </summary>
    public async Task SendMailWithHtmlEmbedded(string senderDisplayName, string senderEmail, string subject, string plainTextBody, string htmlContent, string recipientEmails, string? receipintDisplayName = null)
    {
        var emailMessage = new EmailMessage
        {
            SenderDisplayName = senderDisplayName,
            SenderEmail = senderEmail,
            Subject = subject,
            PlainTextBody = plainTextBody,
            HtmlContent = htmlContent,
            RecipientEmails = ParseRecipientEmails(recipientEmails),
            RecipientDisplayName = receipintDisplayName
        };

        await SendEmailAsync(emailMessage, EmailComposer.ComposeEmbeddedHtmlEmail);
    }

    /// <summary>
    /// Sends an email with attachments
    /// </summary>
    public async Task SendMailWtAttachement(string senderDisplayName, string senderEmail, string subject, string body, string recipientEmails, string? receipintDisplayName = null, params (byte[] content, string fileName)[] attachments)
    {
        var emailAttachments = attachments
            .Where(a => a.content?.Length > 0 && !string.IsNullOrWhiteSpace(a.fileName))
            .Select(a => new EmailAttachment
            {
                Content = a.content,
                FileName = a.fileName
            })
            .ToList();

        var emailMessage = new EmailMessage
        {
            SenderDisplayName = senderDisplayName,
            SenderEmail = senderEmail,
            Subject = subject,
            HtmlBody = body,
            RecipientEmails = ParseRecipientEmails(recipientEmails),
            RecipientDisplayName = receipintDisplayName,
            Attachments = emailAttachments
        };

        await SendEmailAsync(emailMessage, (msg, logger) => EmailComposer.ComposeBasicEmail(msg));
    }

    /// <summary>
    /// Sends an email with embedded HTML content and also attaches the HTML as a file
    /// </summary>
    public async Task SendMailWithHtmlEmbeddedAndAttached(string senderDisplayName, string senderEmail, string subject, string plainTextBody, string htmlContent, string recipientEmails, string? receipintDisplayName = null, string? attachmentFileName = null)
    {
        var emailMessage = new EmailMessage
        {
            SenderDisplayName = senderDisplayName,
            SenderEmail = senderEmail,
            Subject = subject,
            PlainTextBody = plainTextBody,
            HtmlContent = htmlContent,
            RecipientEmails = ParseRecipientEmails(recipientEmails),
            RecipientDisplayName = receipintDisplayName
        };

        await SendEmailAsync(emailMessage, 
            (msg, logger) => EmailComposer.ComposeEmbeddedHtmlEmailWithAttachment(msg, attachmentFileName, logger));
    }

    /// <summary>
    /// Core email sending method with proper error handling and logging
    /// </summary>
    private async Task SendEmailAsync(EmailMessage emailMessage, Func<EmailMessage, ILogger?, MailMessage> composer)
    {
        // Validate email message
        var validation = EmailValidator.ValidateEmailMessage(emailMessage);
        if (!validation.IsValid)
        {
            var errors = string.Join(", ", validation.Errors);
            _logger.LogWarning($"Email validation failed for subject '{emailMessage.Subject}': {errors}");
            return;
        }

        try
        {
            using var smtpClient = _smtpClientFactory.CreateSmtpClient(_emailConfig, emailMessage.SenderEmail);
            using var mailMessage = composer(emailMessage, _logger);

            await smtpClient.SendMailAsync(mailMessage);

            _logger.LogInformation(
                "Email sent successfully from {SenderEmail} to {RecipientEmails} with subject '{Subject}' at {Timestamp}",
                emailMessage.HardCodedSenderEmail,
                string.Join(", ", emailMessage.RecipientEmails),
                emailMessage.Subject,
                DateTime.Now);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to send email from {SenderEmail} to {RecipientEmails} with subject '{Subject}' at {Timestamp}",
                emailMessage.HardCodedSenderEmail,
                string.Join(", ", emailMessage.RecipientEmails),
                emailMessage.Subject,
                DateTime.Now);
        }
    }

    /// <summary>
    /// Parses comma/semicolon separated recipient emails
    /// </summary>
    private static string[] ParseRecipientEmails(string recipientEmails)
    {
        if (string.IsNullOrWhiteSpace(recipientEmails))
            return Array.Empty<string>();

        return recipientEmails
            .Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(email => email.Trim())
            .Where(email => !string.IsNullOrWhiteSpace(email))
            .ToArray();
    }

    /// <summary>
    /// Loads email configuration from app settings
    /// </summary>
    private EmailConfiguration LoadEmailConfiguration(IConfiguration configuration)
    {
        try
        {
            string mailConfigJson = configuration["EmailConfig:EmailConfigType"] == "PLAIN"
                ? configuration["EmailConfig:JsonValue"]
                : RepositoryHelper.EncryptionHelper.UnFist(configuration["EmailConfig:JsonValue"]);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var mailConfig = JsonSerializer.Deserialize<EmailConfigValue>(mailConfigJson, options);

            return new EmailConfiguration
            {
                Password = mailConfig.Password,
                Host = mailConfig.Host,
                Port = mailConfig.Port
            };
        }
        catch (Exception ex )
        {
           _logger.LogCritical(ex, "Failed to load email configuration");
            throw new Exception("Unable to load Email Configuration");
        }
    }
}

/// <summary>
/// Extension methods for easy migration from old MailSender
/// </summary>
public static class MailSenderExtensions
{
    /// <summary>
    /// Sends a simple email with just HTML body
    /// </summary>
    public static async Task SendSimpleHtmlEmailAsync(this MailSenderV2 mailSender, 
        string senderDisplayName, string senderEmail, string subject, string htmlBody, 
        string[] recipientEmails, string? recipientDisplayName = null)
    {
        await mailSender.SendRichHtmlMail(senderDisplayName, senderEmail, subject, htmlBody, 
            string.Join(",", recipientEmails), recipientDisplayName);
    }

    /// <summary>
    /// Sends an email with multiple typed attachments
    /// </summary>
    public static async Task SendEmailWithAttachmentsAsync(this MailSenderV2 mailSender,
        string senderDisplayName, string senderEmail, string subject, string body,
        string[] recipientEmails, IEnumerable<EmailAttachment> attachments, 
        string? recipientDisplayName = null)
    {
        var attachmentTuples = attachments.Select(a => (a.Content, a.FileName)).ToArray();
        await mailSender.SendMailWtAttachement(senderDisplayName, senderEmail, subject, body,
            string.Join(",", recipientEmails), recipientDisplayName, attachmentTuples);
    }
}