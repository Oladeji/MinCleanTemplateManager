//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System.Net;
//using System.Net.Mail;
//using System.Net.Mime;
//using System.Text;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace MailSenderLib;

///// <summary>
///// Enhanced email sender with support for rich HTML, plain text, and multipart emails
///// 
///// NOTE: This is the legacy implementation. For new projects, consider using MailSenderV2
///// which provides better separation of concerns, improved testability, and cleaner architecture.
///// </summary>
//[Obsolete("Consider using MailSenderV2 for new implementations. This class is maintained for backward compatibility.")]
//public class MailSender : IMailSender
//{
//    private readonly ILogger<MailSender> _logger;
//    private readonly EmailConfigValue _emailConfig;

//    public MailSender(ILogger<MailSender> logger, IConfiguration configuration)
//    {
//        var preConfig = configuration.GetSection("EmailConfig");

//        string _mailConfigJson = configuration["EmailConfig:EmailConfigType"] == "PLAIN" ? configuration["EmailConfig:JsonValue"] : RepositoryHelper.EncryptionHelper.UnFist(configuration["EmailConfig:JsonValue"]);

//        var options = new JsonSerializerOptions
//        {
//            PropertyNameCaseInsensitive = true,
//            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
//        };
//        var _mailConfig = JsonSerializer.Deserialize<EmailConfigValue>(_mailConfigJson, options);
//        _logger = logger;

//        _emailConfig = new EmailConfigValue()
//        {
//            Password = _mailConfig.Password,
//            Host = _mailConfig.Host,
//            Port = _mailConfig.Port,
//        };
//    }

//    /// <summary>
//    /// Sends a basic email (delegates to SendRichHtmlMail for backward compatibility)
//    /// </summary>
//    public Task SendMail(string senderDisplayName, string senderEmail, string subject, string body, string recipientEmails, string? receipintDisplayName = null)
//    {
//        return SendRichHtmlMail(senderDisplayName, senderEmail, subject, body, recipientEmails, receipintDisplayName);
//    }

//    /// <summary>
//    /// Enhanced HTML email sender with better MIME support and fallback text
//    /// This creates a multipart/alternative email where the HTML is the primary content
//    /// </summary>
//    public Task SendRichHtmlMail(string senderDisplayName, string senderEmail, string subject, string htmlBody, string recipientEmails, string? receipintDisplayName = null, string? plainTextFallback = null)
//    {
//        if (recipientEmails == "" || recipientEmails == null || senderEmail == "" || senderEmail == null)
//        {
//            _logger.LogInformation($"recipientEmails or senderEmail email is empty for mail with subject {subject} sent on {DateTime.Now}");
//            return Task.CompletedTask;
//        }

//        try
//        {
//            var fromAddress = new MailAddress(senderEmail, senderDisplayName);
//            var smtp = new SmtpClient
//            {
//                Host = _emailConfig.Host,
//                Port = _emailConfig.Port,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(fromAddress.Address, _emailConfig.Password),
//                Timeout = 20000
//            };

//            using (var message = new MailMessage()
//            {
//                From = fromAddress,
//                Subject = subject,
//            })
//            {
//                if (string.IsNullOrWhiteSpace(recipientEmails))
//                {
//                    _logger.LogError($"Failed to send email from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: Recipient email address is empty");
//                    return Task.CompletedTask;
//                }

//                var recipientAddresses = recipientEmails.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
//                foreach (var recipientAddress in recipientAddresses)
//                {
//                    message.To.Add(new MailAddress(recipientAddress.Trim(), receipintDisplayName ?? recipientAddress.Trim()));
//                }

//                // Add sender as BCC to keep a copy in their sent folder
//                message.CC.Add(new MailAddress(senderEmail, "A Copy of " + subject));

//                // Create multipart/alternative message for better HTML email support
//                if (!string.IsNullOrEmpty(plainTextFallback))
//                {
//                    // Create plain text view
//                    var plainTextView = AlternateView.CreateAlternateViewFromString(
//                        plainTextFallback,
//                        Encoding.UTF8,
//                        MediaTypeNames.Text.Plain);

//                    // Create HTML view with proper content type
//                    var htmlView = AlternateView.CreateAlternateViewFromString(
//                        htmlBody,
//                        Encoding.UTF8,
//                        MediaTypeNames.Text.Html);

//                    message.AlternateViews.Add(plainTextView);
//                    message.AlternateViews.Add(htmlView);
//                }
//                else
//                {
//                    // Simple HTML body (current approach)
//                    message.Body = htmlBody;
//                    message.IsBodyHtml = true;
//                }

//                // Set encoding for better international character support
//                message.SubjectEncoding = Encoding.UTF8;
//                message.BodyEncoding = Encoding.UTF8;

//                smtp.Send(message);
//            }

//            _logger.LogInformation($"Rich HTML email sent successfully from {senderEmail} to {recipientEmails} on {DateTime.Now}");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Failed to send rich HTML email from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: {ex}");
//        }

//        return Task.CompletedTask;
//    }

//    /// <summary>
//    /// Sends an email with both a plain text body and HTML content as separate parts in a multipart/mixed message.
//    /// This approach provides:
//    /// - A plain text message that appears first in the email
//    /// - Rich HTML content embedded as an additional part below the plain text
//    /// - Both contents are visible simultaneously in supporting email clients
//    /// 
//    /// Use this when you want recipients to see both a summary message in plain text
//    /// AND detailed HTML content in the same email body.
//    /// </summary>
//    public Task SendMailWithHtmlEmbedded(string senderDisplayName, string senderEmail, string subject, string plainTextBody, string htmlContent, string recipientEmails, string? receipintDisplayName = null)
//    {
//        if (recipientEmails == "" || recipientEmails == null || senderEmail == "" || senderEmail == null)
//        {
//            _logger.LogInformation($"recipientEmails or senderEmail email is empty for mail with subject {subject} sent on {DateTime.Now}");
//            return Task.CompletedTask;
//        }

//        try
//        {
//            var fromAddress = new MailAddress(senderEmail, senderDisplayName);
//            var smtp = new SmtpClient
//            {
//                Host = _emailConfig.Host,
//                Port = _emailConfig.Port,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(fromAddress.Address, _emailConfig.Password),
//                Timeout = 20000
//            };

//            using (var message = new MailMessage()
//            {
//                From = fromAddress,
//                Subject = subject,
//            })
//            {
//                if (string.IsNullOrWhiteSpace(recipientEmails))
//                {
//                    _logger.LogError($"Failed to send email from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: Recipient email address is empty");
//                    return Task.CompletedTask;
//                }

//                var recipientAddresses = recipientEmails.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
//                foreach (var recipientAddress in recipientAddresses)
//                {
//                    message.To.Add(new MailAddress(recipientAddress.Trim(), receipintDisplayName ?? recipientAddress.Trim()));
//                }

//                // Add sender as BCC to keep a copy in their sent folder
//                message.CC.Add(new MailAddress(senderEmail, "A Copy of " + subject));

//                // Extract body content and CSS styles from the complete HTML document to avoid nested HTML structure
//                var (extractedHtmlContent, extractedStyles) = EmailHtmlUtils.ExtractBodyContentAndStylesFromHtml(htmlContent, _logger);

//                // Create a combined body that includes both plain text and HTML with proper 60% width containers
//                var combinedHtmlBody = EmailHtmlUtils.BuildCombinedEmailHtmlBody(plainTextBody, extractedHtmlContent, extractedStyles);

//                // Set the combined content as the main HTML body
//                message.Body = combinedHtmlBody;
//                message.IsBodyHtml = true;

//                // Also add plain text as an alternative view for accessibility
//                var plainTextView = AlternateView.CreateAlternateViewFromString(
//                    plainTextBody,
//                    Encoding.UTF8,
//                    MediaTypeNames.Text.Plain);

//                var htmlView = AlternateView.CreateAlternateViewFromString(
//                    combinedHtmlBody,
//                    Encoding.UTF8,
//                    MediaTypeNames.Text.Html);

//                message.AlternateViews.Add(plainTextView);
//                message.AlternateViews.Add(htmlView);

//                // Set encoding for better international character support
//                message.SubjectEncoding = Encoding.UTF8;
//                message.BodyEncoding = Encoding.UTF8;

//                smtp.Send(message);
//            }

//            _logger.LogInformation($"Email with combined plain text and HTML content sent successfully from {senderEmail} to {recipientEmails} on {DateTime.Now}");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Failed to send email with combined plain text and HTML content from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: {ex}");
//        }

//        return Task.CompletedTask;
//    }

//    public Task SendMailWtAttachement(string senderDisplayName, string senderEmail, string subject, string body, string recipientEmails, string? receipintDisplayName = null, params (byte[] content, string fileName)[] attachments)
//    {
//        if (recipientEmails == "" || recipientEmails == null || senderEmail == "" || senderEmail == null)
//        {
//            _logger.LogInformation($"recipientEmails or senderEmail is empty for mail with subject {subject} sent on {DateTime.Now}");
//            return Task.CompletedTask;
//        }

//        try
//        {
//            var fromAddress = new MailAddress(senderEmail, senderDisplayName);
//            var smtp = new SmtpClient
//            {
//                Host = _emailConfig.Host,
//                Port = _emailConfig.Port,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(fromAddress.Address, _emailConfig.Password),
//                Timeout = 20000
//            };

//            using (var message = new MailMessage()
//            {
//                From = fromAddress,
//                Subject = subject,
//                Body = body,
//                IsBodyHtml = true,
//            })
//            {
//                if (string.IsNullOrWhiteSpace(recipientEmails))
//                {
//                    _logger.LogError($"Failed to send email from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: Recipient email address is empty");
//                    return Task.CompletedTask;
//                }

//                // Add recipients
//                var recipientAddresses = recipientEmails.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
//                foreach (var recipientAddress in recipientAddresses)
//                {
//                    message.To.Add(new MailAddress(recipientAddress.Trim(), receipintDisplayName ?? recipientAddress.Trim()));
//                }
//                // Add sender as BCC to keep a copy in their sent folder
//                message.CC.Add(new MailAddress(senderEmail, "A Copy of " + subject));

//                // Set encoding for better international character support
//                message.SubjectEncoding = Encoding.UTF8;
//                message.BodyEncoding = Encoding.UTF8;

//                // Add attachments from byte arrays
//                if (attachments != null && attachments.Length > 0)
//                {
//                    foreach (var (content, fileName) in attachments)
//                    {
//                        if (content != null && content.Length > 0 && !string.IsNullOrWhiteSpace(fileName))
//                        {
//                            try
//                            {
//                                var stream = new MemoryStream(content);
//                                var attachment = new Attachment(stream, fileName);
//                                message.Attachments.Add(attachment);
//                                _logger.LogInformation($"Added attachment: {fileName} ({content.Length} bytes)");
//                            }
//                            catch (Exception attachEx)
//                            {
//                                _logger.LogWarning($"Failed to add attachment {fileName}: {attachEx.Message}");
//                            }
//                        }
//                        else
//                        {
//                            _logger.LogWarning($"Invalid attachment data or filename: {fileName}");
//                        }
//                    }
//                }

//                smtp.Send(message);
//            }

//            _logger.LogInformation($"Email with attachments sent successfully from {senderEmail} to {recipientEmails} on {DateTime.Now}");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Failed to send email with attachments from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: {ex}");
//        }

//        return Task.CompletedTask;
//    }

//    /// <summary>
//    /// Sends an email with both a plain text body and HTML content embedded in the email body,
//    /// AND also attaches the HTML report as a separate file attachment.
//    /// This approach provides:
//    /// - A plain text message that appears first in the email
//    /// - Rich HTML content embedded as an additional part below the plain text
//    /// - The HTML report also attached as a downloadable file
//    /// - Maximum compatibility and flexibility for recipients
//    /// 
//    /// Use this when you want recipients to see both a summary message in plain text,
//    /// detailed HTML content in the email body, AND have the option to save the report as a file.
//    /// </summary>
//    public Task SendMailWithHtmlEmbeddedAndAttached(string senderDisplayName, string senderEmail, string subject, string plainTextBody, string htmlContent, string recipientEmails, string? receipintDisplayName = null, string? attachmentFileName = null)
//    {
//        if (recipientEmails == "" || recipientEmails == null || senderEmail == "" || senderEmail == null)
//        {
//            _logger.LogInformation($"recipientEmails or senderEmail email is empty for mail with subject {subject} sent on {DateTime.Now}");
//            return Task.CompletedTask;
//        }

//        try
//        {
//            var fromAddress = new MailAddress(senderEmail, senderDisplayName);
//            var smtp = new SmtpClient
//            {
//                Host = _emailConfig.Host,
//                Port = _emailConfig.Port,
//                EnableSsl = true,
//                DeliveryMethod = SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(fromAddress.Address, _emailConfig.Password),
//                Timeout = 20000
//            };

//            using (var message = new MailMessage()
//            {
//                From = fromAddress,
//                Subject = subject,
//            })
//            {
//                if (string.IsNullOrWhiteSpace(recipientEmails))
//                {
//                    _logger.LogError($"Failed to send email from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: Recipient email address is empty");
//                    return Task.CompletedTask;
//                }

//                var recipientAddresses = recipientEmails.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
//                foreach (var recipientAddress in recipientAddresses)
//                {
//                    message.To.Add(new MailAddress(recipientAddress.Trim(), recipientAddress.Trim()));
//                }

//                // Add sender as BCC to keep a copy in their sent folder
//                //  message.Bcc.Add(new MailAddress(senderEmail, senderDisplayName));
//                // Use CC instead of BCC for transparency
//                message.CC.Add(new MailAddress(senderEmail, "A Copy of " + subject));
                
//                // Extract body content and CSS styles from the complete HTML document to avoid nested HTML structure
//                var (extractedHtmlContent, extractedStyles) = EmailHtmlUtils.ExtractBodyContentAndStylesFromHtml(htmlContent, _logger);
//                string combinedHtmlBody = EmailHtmlUtils.BuildCombinedEmailHtmlBody(plainTextBody, extractedHtmlContent, extractedStyles);

//                // Set the combined content as the main HTML body
//                message.Body = combinedHtmlBody;
//                message.IsBodyHtml = true;

//                // Also add plain text as an alternative view for accessibility
//                var plainTextView = AlternateView.CreateAlternateViewFromString(
//                    plainTextBody,
//                    Encoding.UTF8,
//                    MediaTypeNames.Text.Plain);

//                var htmlView = AlternateView.CreateAlternateViewFromString(
//                    combinedHtmlBody,
//                    Encoding.UTF8,
//                    MediaTypeNames.Text.Html);

//                message.AlternateViews.Add(plainTextView);
//                message.AlternateViews.Add(htmlView);

//                // Add the original HTML report as an attachment (not the combined email body)
//                if (!string.IsNullOrEmpty(htmlContent))
//                {
//                    try
//                    {
//                        var htmlBytes = System.Text.Encoding.UTF8.GetBytes(htmlContent);
//                        var fileName = attachmentFileName ?? $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.html";

//                        var stream = new MemoryStream(htmlBytes);
//                        var attachment = new Attachment(stream, fileName, MediaTypeNames.Text.Html);
//                        message.Attachments.Add(attachment);
//                        _logger.LogInformation($"Added HTML report attachment: {fileName} ({htmlBytes.Length} bytes)");
//                    }
//                    catch (Exception attachEx)
//                    {
//                        _logger.LogWarning($"Failed to add HTML report attachment: {attachEx.Message}");
//                    }
//                }

//                // Set encoding for better international character support
//                message.SubjectEncoding = Encoding.UTF8;
//                message.BodyEncoding = Encoding.UTF8;

//                smtp.Send(message);
//            }

//            _logger.LogInformation($"Email with embedded HTML content and attachment sent successfully from {senderEmail} to {recipientEmails} on {DateTime.Now}");
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError($"Failed to send email with embedded HTML content and attachment from {senderEmail} to {recipientEmails} on {DateTime.Now} with the following error: {ex}");
//        }

//        return Task.CompletedTask;
//    }
//}







