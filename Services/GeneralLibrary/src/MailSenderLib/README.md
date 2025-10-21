# MailSender Library Refactoring

## Overview

The MailSender library has been refactored to provide a cleaner, more maintainable, and testable email sending solution. The refactoring introduces several new classes while maintaining backward compatibility with the existing `MailSender` class.

## New Architecture

### Core Components

1. **EmailCore.cs** - Contains the foundational types and utilities:
   - `EmailConfiguration` - Configuration record for SMTP settings
   - `EmailMessage` - Represents an email message to be sent
   - `EmailAttachment` - Represents email attachments
   - `EmailValidator` - Validates email messages before sending
   - `ISmtpClientFactory` & `SmtpClientFactory` - Factory for creating SMTP clients
   - `EmailComposer` - Handles email composition logic

2. **MailSenderV2.cs** - The new, improved email sender implementation
   - Clean separation of concerns
   - Better error handling and logging
   - Improved testability through dependency injection
   - Async/await pattern throughout

3. **EmailHtmlUtils.cs** - Static utility class for HTML processing (already existing)
   - Moved from private methods in MailSender to a reusable static class
   - HTML content extraction and CSS prefix handling

## Key Improvements

### 1. **Separation of Concerns**
- **Validation** is handled by `EmailValidator`
- **Composition** is handled by `EmailComposer`
- **SMTP Client Creation** is handled by `SmtpClientFactory`
- **Configuration** is encapsulated in `EmailConfiguration`

### 2. **Better Testability**
- Dependencies are injected via constructor
- SMTP client creation is abstracted through `ISmtpClientFactory`
- Each component can be unit tested independently

### 3. **Improved Error Handling**
- Structured logging with proper log levels
- Comprehensive validation before sending
- Better exception handling and recovery

### 4. **Cleaner API**
- Strongly typed configuration and message objects
- Extension methods for common scenarios
- Async/await pattern for better performance

### 5. **Reduced Code Duplication**
- Common email logic extracted to `EmailComposer`
- SMTP setup logic centralized in factory
- Validation logic reused across all methods

## Usage Examples

### Basic Email Sending
```csharp
// Using MailSenderV2
var logger = serviceProvider.GetService<ILogger<MailSenderV2>>();
var configuration = serviceProvider.GetService<IConfiguration>();
var mailSender = new MailSenderV2(logger, configuration);

await mailSender.SendRichHtmlMail(
    "John Doe", 
    "john@example.com", 
    "Test Subject", 
    "<h1>Hello World</h1>", 
    "recipient@example.com"
);
```

### Email with Attachments
```csharp
var attachments = new[]
{
    (File.ReadAllBytes("document.pdf"), "document.pdf"),
    (File.ReadAllBytes("image.jpg"), "image.jpg")
};

await mailSender.SendMailWtAttachement(
    "John Doe",
    "john@example.com",
    "Documents Attached",
    "<p>Please find the attached documents.</p>",
    "recipient@example.com",
    null,
    attachments
);
```

### Using Extension Methods
```csharp
// Send to multiple recipients with typed attachments
var recipients = new[] { "user1@example.com", "user2@example.com" };
var attachments = new[]
{
    new EmailAttachment 
    { 
        Content = pdfBytes, 
        FileName = "report.pdf", 
        ContentType = "application/pdf" 
    }
};

await mailSender.SendEmailWithAttachmentsAsync(
    "System", 
    "system@example.com", 
    "Monthly Report", 
    "<h1>Monthly Report</h1>", 
    recipients, 
    attachments
);
```

## Migration Guide

### For New Projects
Use `MailSenderV2` for all new implementations:

```csharp
// Register in DI container
services.AddScoped<ISmtpClientFactory, SmtpClientFactory>();
services.AddScoped<MailSenderV2>();
```

### For Existing Projects
The original `MailSender` class remains unchanged and fully backward compatible. You can:

1. **Immediate migration**: Replace `MailSender` with `MailSenderV2` in your DI registration
2. **Gradual migration**: Keep using `MailSender` and migrate individual usages over time
3. **Side-by-side**: Use both implementations during transition period

## Testing

The new architecture is designed for testability:

```csharp
[Test]
public async Task SendEmail_ValidMessage_Success()
{
    // Arrange
    var mockLogger = new Mock<ILogger<MailSenderV2>>();
    var mockSmtpFactory = new Mock<ISmtpClientFactory>();
    var mockSmtpClient = new Mock<SmtpClient>();
    
    mockSmtpFactory.Setup(f => f.CreateSmtpClient(It.IsAny<EmailConfiguration>(), It.IsAny<string>()))
               .Returns(mockSmtpClient.Object);
    
    var mailSender = new MailSenderV2(mockLogger.Object, configuration, mockSmtpFactory.Object);
    
    // Act
    await mailSender.SendRichHtmlMail("Test", "test@example.com", "Subject", "<h1>Test</h1>", "recipient@example.com");
    
    // Assert
    mockSmtpClient.Verify(s => s.SendMailAsync(It.IsAny<MailMessage>()), Times.Once);
}
```

## Performance Improvements

1. **Async/Await**: All methods now use async/await for better scalability
2. **Reduced Allocations**: Better memory management with reusable components
3. **Early Validation**: Fail fast with comprehensive validation before expensive operations
4. **Resource Management**: Proper disposal of SMTP clients and mail messages

## Configuration

The configuration structure remains the same, ensuring existing deployments continue to work without changes:

```json
{
  "EmailConfig": {
    "EmailConfigType": "PLAIN",
    "JsonValue": "{\"Host\":\"smtp.gmail.com\",\"Port\":587,\"Password\":\"your-password\"}"
  }
}
```

## Backward Compatibility

- The original `MailSender` class is marked with `[Obsolete]` but remains fully functional
- All existing method signatures are preserved
- No breaking changes to the public API
- Configuration format remains unchanged

## Future Enhancements

The new architecture enables several future improvements:
- **Templating System**: Easy to add email template support
- **Retry Logic**: Can be added to the SMTP factory
- **Multiple Providers**: Support for different email providers
- **Metrics and Monitoring**: Built-in support for telemetry
- **Batch Sending**: Efficient bulk email operations