using Microsoft.Extensions.Logging;
using System.Net.Mail;

namespace MailSenderLib;

/// <summary>
/// Represents email configuration and credentials
/// </summary>
public record EmailConfiguration
{
    public required string Host { get; init; }
    public required int Port { get; init; }
    public required string Password { get; init; }
    public bool EnableSsl { get; init; } = true;
    public int TimeoutMs { get; init; } = 20000;
}

/// <summary>
/// Represents an email message to be sent
/// </summary>
public record EmailMessage
{

    public  string HardCodedDisplayName { get; init; }="Massfusion";
    public  string HardCodedSenderEmail { get; init; }= "Massfusion@massload.com";
    public required string SenderDisplayName { get; init; }
    public required string SenderEmail { get; init; }
    public required string Subject { get; init; }
    public required string[] RecipientEmails { get; init; }
    public string? RecipientDisplayName { get; init; }
    public string? PlainTextBody { get; init; }
    public string? HtmlBody { get; init; }
    public string? HtmlContent { get; init; }
    public List<EmailAttachment> Attachments { get; init; } = new();
    public bool IncludeSenderCopy { get; init; } = true;
}

/// <summary>
/// Represents an email attachment
/// </summary>
public record EmailAttachment
{
    public required byte[] Content { get; init; }
    public required string FileName { get; init; }
    public string? ContentType { get; init; }
}

/// <summary>
/// Handles email validation logic
/// </summary>
public static class EmailValidator
{
    public static ValidationResult ValidateEmailMessage(EmailMessage emailMessage)
    {
        var errors = new List<string>();

        //if (string.IsNullOrWhiteSpace(emailMessage.SenderEmail))
        //    errors.Add("Sender email cannot be empty");

        if (emailMessage.RecipientEmails.Length == 0 || 
            emailMessage.RecipientEmails.All(string.IsNullOrWhiteSpace))
            errors.Add("At least one recipient email must be provided");

        if (string.IsNullOrWhiteSpace(emailMessage.Subject))
            errors.Add("Subject cannot be empty");

        if (string.IsNullOrWhiteSpace(emailMessage.PlainTextBody) && 
            string.IsNullOrWhiteSpace(emailMessage.HtmlBody) && 
            string.IsNullOrWhiteSpace(emailMessage.HtmlContent))
            errors.Add("Email must have either plain text body, HTML body, or HTML content");

        //// Validate email addresses
        //try
        //{
        //    var _ = new MailAddress(emailMessage.SenderEmail);
        //}
        //catch
        //{
        //    errors.Add($"Invalid sender email address: {emailMessage.SenderEmail}");
        //}

        foreach (var recipient in emailMessage.RecipientEmails.Where(r => !string.IsNullOrWhiteSpace(r)))
        {
            try
            {
                var _ = new MailAddress(recipient);
            }
            catch
            {
                errors.Add($"Invalid recipient email address: {recipient}");
            }
        }

        return new ValidationResult(errors.Count == 0, errors);
    }
}

/// <summary>
/// Represents validation result
/// </summary>
public record ValidationResult(bool IsValid, List<string> Errors);

/// <summary>
/// Factory for creating SMTP clients
/// </summary>
public interface ISmtpClientFactory
{
    SmtpClient CreateSmtpClient(EmailConfiguration config, string senderEmail);
}

public class SmtpClientFactory : ISmtpClientFactory
{
    public SmtpClient CreateSmtpClient(EmailConfiguration config, string senderEmail)
    {
        return new SmtpClient
        {
            Host = config.Host,
            Port = config.Port,
            EnableSsl = config.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new System.Net.NetworkCredential(senderEmail, config.Password),
            Timeout = config.TimeoutMs
        };
    }
}

/// <summary>
/// Handles email composition logic
/// </summary>
public static class EmailComposer
{
    public static MailMessage ComposeBasicEmail(EmailMessage emailMessage)
    {
        var mailMessage = CreateBaseMailMessage(emailMessage);
        
        if (!string.IsNullOrEmpty(emailMessage.HtmlBody))
        {
            mailMessage.Body = emailMessage.HtmlBody;
            mailMessage.IsBodyHtml = true;
        }
        else if (!string.IsNullOrEmpty(emailMessage.PlainTextBody))
        {
            mailMessage.Body = emailMessage.PlainTextBody;
            mailMessage.IsBodyHtml = false;
        }

        AddAttachments(mailMessage, emailMessage.Attachments);
        return mailMessage;
    }

    public static MailMessage ComposeRichHtmlEmail(EmailMessage emailMessage, ILogger? logger = null)
    {
        var mailMessage = CreateBaseMailMessage(emailMessage);

        if (!string.IsNullOrEmpty(emailMessage.PlainTextBody))
        {
            // Create multipart/alternative message
            var plainTextView = AlternateView.CreateAlternateViewFromString(
                emailMessage.PlainTextBody,
                System.Text.Encoding.UTF8,
                System.Net.Mime.MediaTypeNames.Text.Plain);

            var htmlView = AlternateView.CreateAlternateViewFromString(
                emailMessage.HtmlBody ?? "",
                System.Text.Encoding.UTF8,
                System.Net.Mime.MediaTypeNames.Text.Html);

            mailMessage.AlternateViews.Add(plainTextView);
            mailMessage.AlternateViews.Add(htmlView);
        }
        else
        {
            mailMessage.Body = emailMessage.HtmlBody ?? "";
            mailMessage.IsBodyHtml = true;
        }

        AddAttachments(mailMessage, emailMessage.Attachments);
        return mailMessage;
    }

    public static MailMessage ComposeEmbeddedHtmlEmail(EmailMessage emailMessage, ILogger? logger = null)
    {
        var mailMessage = CreateBaseMailMessage(emailMessage);

        // Extract HTML content and styles
        var (extractedHtmlContent, extractedStyles) = 
            EmailHtmlUtils.ExtractBodyContentAndStylesFromHtml(emailMessage.HtmlContent ?? "", logger);

        // Build combined HTML body
        var combinedHtmlBody = EmailHtmlUtils.BuildCombinedEmailHtmlBody(
            emailMessage.PlainTextBody ?? "", extractedHtmlContent, extractedStyles);

        mailMessage.Body = combinedHtmlBody;
        mailMessage.IsBodyHtml = true;

        // Add alternative views
        if (!string.IsNullOrEmpty(emailMessage.PlainTextBody))
        {
            var plainTextView = AlternateView.CreateAlternateViewFromString(
                emailMessage.PlainTextBody,
                System.Text.Encoding.UTF8,
                System.Net.Mime.MediaTypeNames.Text.Plain);

            var htmlView = AlternateView.CreateAlternateViewFromString(
                combinedHtmlBody,
                System.Text.Encoding.UTF8,
                System.Net.Mime.MediaTypeNames.Text.Html);

            mailMessage.AlternateViews.Add(plainTextView);
            mailMessage.AlternateViews.Add(htmlView);
        }

        AddAttachments(mailMessage, emailMessage.Attachments);
        return mailMessage;
    }

    public static MailMessage ComposeEmbeddedHtmlEmailWithAttachment(EmailMessage emailMessage, string? attachmentFileName = null, ILogger? logger = null)
    {
        var mailMessage = ComposeEmbeddedHtmlEmail(emailMessage, logger);

        // Add the original HTML content as attachment
        if (!string.IsNullOrEmpty(emailMessage.HtmlContent))
        {
            var fileName = attachmentFileName ?? $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            var htmlBytes = System.Text.Encoding.UTF8.GetBytes(emailMessage.HtmlContent);
            
            var attachment = new EmailAttachment
            {
                Content = htmlBytes,
                FileName = fileName,
                ContentType = System.Net.Mime.MediaTypeNames.Text.Html
            };

            var attachments = emailMessage.Attachments.ToList();
            attachments.Add(attachment);
            
            AddAttachments(mailMessage, attachments);
        }

        return mailMessage;
    }

    private static MailMessage CreateBaseMailMessage(EmailMessage emailMessage)
    {
       // var fromAddress = new MailAddress(emailMessage.SenderEmail, emailMessage.SenderDisplayName);
        var fromAddress = new MailAddress(emailMessage.HardCodedSenderEmail, emailMessage.HardCodedDisplayName);
        var mailMessage = new MailMessage
        {
            From = fromAddress,
            Subject = emailMessage.Subject,
            SubjectEncoding = System.Text.Encoding.UTF8,
            BodyEncoding = System.Text.Encoding.UTF8
        };

        // Add recipients
        foreach (var recipient in emailMessage.RecipientEmails.Where(r => !string.IsNullOrWhiteSpace(r)))
        {
            //var displayName = emailMessage.RecipientEmails ?? recipient.Trim();
            mailMessage.To.Add(new MailAddress(recipient.Trim()));
        }

        // Add sender copy if requested
        if (emailMessage.IncludeSenderCopy)
        {
            mailMessage.CC.Add(new MailAddress(emailMessage.SenderEmail, $"A Copy of {emailMessage.Subject}"));
        }

        return mailMessage;
    }

    private static void AddAttachments(MailMessage mailMessage, IEnumerable<EmailAttachment> attachments)
    {
        foreach (var emailAttachment in attachments)
        {
            if (emailAttachment.Content?.Length > 0 && !string.IsNullOrWhiteSpace(emailAttachment.FileName))
            {
                try
                {
                    var stream = new MemoryStream(emailAttachment.Content);
                    var attachment = new Attachment(stream, emailAttachment.FileName, emailAttachment.ContentType);
                    mailMessage.Attachments.Add(attachment);
                }
                catch
                {
                    // Ignore invalid attachments
                }
            }
        }
    }
}