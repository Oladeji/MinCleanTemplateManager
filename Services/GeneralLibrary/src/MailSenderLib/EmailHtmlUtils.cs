using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.RegularExpressions;

namespace MailSenderLib;

/// <summary>
/// Static utility class for HTML email processing and manipulation
/// </summary>
public static class EmailHtmlUtils
{
    /// <summary>
    /// Builds a combined HTML email body that includes both plain text and HTML content
    /// with proper styling and responsive design
    /// </summary>
    /// <param name="plainTextBody">The plain text content to display first</param>
    /// <param name="extractedHtmlContent">The HTML content extracted from the original document</param>
    /// <param name="extractedStyles">The CSS styles extracted from the original document</param>
    /// <returns>A complete HTML document ready for email</returns>
    public static string BuildCombinedEmailHtmlBody(string plainTextBody, string extractedHtmlContent, string extractedStyles)
    {
        return $@"<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <style>
        /* Global reset - applied once */
        * {{
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }}

        /* Email body styles */
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f0f2f5;
            padding: 0;
            line-height: 1.4;
            font-size: 11px;
            color: #333;
        }}

        /* Email Container Styles - Outer container that takes full width */
        .email-outer-container {{
            width: 100%;
            min-height: 100vh;
            background-color: #f0f2f5;
            padding: 0;
            display: flex;
            flex-direction: column;
            align-items: center;
        }}

        /* Email Content wrapper that takes 60% of Container B */
        .email-content-wrapper {{
            width: 60%;
            max-width: 800px;
            min-width: 600px;
            margin: 20px 0;
            padding: 0;
        }}

        /* Plain text section styling */
        .plain-text-section {{
            font-family: monospace;
            background-color: #f5f5f5;
            padding: 15px;
            border-left: 4px solid #007acc;
            margin-bottom: 20px;
            white-space: pre-line;
            border-radius: 4px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            border: 1px solid #e0e0e0;
        }}

        /* HTML content section container */
        .html-content-section {{
            margin-top: 20px;
        }}

        /* Warranty status styling */
        .warranty-status-container {{
            margin-bottom: 15px;
            text-align: center;
        }}

        /* Responsive styles */
        @media (max-width: 800px) {{
            .email-content-wrapper {{
                width: 80%;
                min-width: 320px;
            }}
        }}

        @media (max-width: 500px) {{
            .email-content-wrapper {{
                width: 95%;
            }}
        }}

        /* Extracted and prefixed styles from the original HTML report */
        {extractedStyles}
    </style>
</head>
<body>
    <!-- Email Container - Outer container taking full width -->
    <div class=""email-outer-container"">
        <!-- Email Content wrapper taking 60% of Container width -->
        <div class=""email-content-wrapper"">
            <div class=""plain-text-section"">
{plainTextBody.Replace("\n", "<br>").Replace("  ", "&nbsp;&nbsp;")}
            </div>
            <div class=""html-content-section"">
{extractedHtmlContent}
            </div>
        </div>
    </div>
</body>
</html>";
    }

    /// <summary>
    /// Extracts the body content from a complete HTML document, removing DOCTYPE, html, head, and body tags
    /// to avoid nested HTML structure when embedding in another HTML document.
    /// Also extracts and returns the CSS styles with unique class names to prevent conflicts.
    /// </summary>
    /// <param name="htmlContent">The complete HTML document to process</param>
    /// <param name="logger">Logger instance for warning messages</param>
    /// <returns>A tuple containing the extracted content and styles</returns>
    public static (string extractedContent, string extractedStyles) ExtractBodyContentAndStylesFromHtml(string htmlContent, ILogger? logger = null)
    {
        if (string.IsNullOrEmpty(htmlContent))
            return (htmlContent, "");

        try
        {
            string extractedStyles = "";
            string extractedContent = "";

            // Remove DOCTYPE declaration
            var content = Regex.Replace(htmlContent,
                @"<!DOCTYPE[^>]*>", "", RegexOptions.IgnoreCase);

            // Extract CSS styles from <style> tags in the head section
            var styleMatches = Regex.Matches(content,
                @"<style[^>]*>(.*?)</style>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var styleBuilder = new StringBuilder();
            foreach (Match styleMatch in styleMatches)
            {
                var styleContent = styleMatch.Groups[1].Value;

                // Prefix conflicting class names to make them unique for the report content
                styleContent = PrefixClassNamesInStyles(styleContent, "report-", logger);

                styleBuilder.AppendLine(styleContent);
            }
            extractedStyles = styleBuilder.ToString();

            // Extract content between <body> tags, or if no body tags, extract content between <html> tags
            var bodyMatch = Regex.Match(content,
                @"<body[^>]*>(.*?)</body>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (bodyMatch.Success)
            {
                extractedContent = bodyMatch.Groups[1].Value.Trim();
                // Update class names in the HTML content to match the prefixed styles
                extractedContent = PrefixClassNamesInHtml(extractedContent, "report-", logger);
            }
            else
            {
                // If no body tags, try to extract from html tags
                var htmlMatch = Regex.Match(content,
                    @"<html[^>]*>(.*?)</html>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (htmlMatch.Success)
                {
                    var htmlInnerContent = htmlMatch.Groups[1].Value;

                    // Remove head section if present (but we already extracted styles)
                    htmlInnerContent = Regex.Replace(htmlInnerContent,
                        @"<head[^>]*>.*?</head>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    extractedContent = htmlInnerContent.Trim();
                    // Update class names in the HTML content to match the prefixed styles
                    extractedContent = PrefixClassNamesInHtml(extractedContent, "report-", logger);
                }
                else
                {
                    // If neither body nor html tags found, return the content as-is (after DOCTYPE removal)
                    extractedContent = content.Trim();
                }
            }

            return (extractedContent, extractedStyles);
        }
        catch (Exception ex)
        {
            logger?.LogWarning($"Failed to extract body content and styles from HTML, using original content: {ex.Message}");
            return (htmlContent, "");
        }
    }

    /// <summary>
    /// Prefixes class names in CSS styles to avoid conflicts with email container styles
    /// </summary>
    /// <param name="cssContent">The CSS content to process</param>
    /// <param name="prefix">The prefix to add to conflicting class names</param>
    /// <param name="logger">Logger instance for warning messages</param>
    /// <returns>CSS content with prefixed class names</returns>
    public static string PrefixClassNamesInStyles(string cssContent, string prefix, ILogger? logger = null)
    {
        try
        {
            // List of conflicting class names that need prefixing
            var conflictingClasses = new[] {
                "email-outer-container",
                "email-report-wrapper",
                "email-content-wrapper",
                "report-container",
                "report-header"
            };

            var result = cssContent;
            foreach (var className in conflictingClasses)
            {
                // Replace .className with .prefix-className in CSS selectors
                result = Regex.Replace(result,
                    @"\." + Regex.Escape(className) + @"\b",
                    "." + prefix + className,
                    RegexOptions.IgnoreCase);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger?.LogWarning($"Failed to prefix class names in styles: {ex.Message}");
            return cssContent;
        }
    }

    /// <summary>
    /// Prefixes class names in HTML content to match the prefixed CSS styles
    /// </summary>
    /// <param name="htmlContent">The HTML content to process</param>
    /// <param name="prefix">The prefix to add to conflicting class names</param>
    /// <param name="logger">Logger instance for warning messages</param>
    /// <returns>HTML content with prefixed class names</returns>
    public static string PrefixClassNamesInHtml(string htmlContent, string prefix, ILogger? logger = null)
    {
        try
        {
            // List of conflicting class names that need prefixing
            var conflictingClasses = new[] {
                "email-outer-container",
                "email-report-wrapper",
                "email-content-wrapper",
                "report-container",
                "report-header"
            };

            var result = htmlContent;
            foreach (var className in conflictingClasses)
            {
                // Replace class="className" with class="prefix-className" in HTML
                result = Regex.Replace(result,
                    @"class=""([^""]*\b)" + Regex.Escape(className) + @"\b([^""]*)""",
                    @"class=""$1" + prefix + className + @"$2""",
                    RegexOptions.IgnoreCase);

                // Also handle single quotes
                result = Regex.Replace(result,
                    @"class='([^']*\b)" + Regex.Escape(className) + @"\b([^']*)'",
                    @"class='$1" + prefix + className + @"$2'",
                    RegexOptions.IgnoreCase);
            }

            return result;
        }
        catch (Exception ex)
        {
            logger?.LogWarning($"Failed to prefix class names in HTML: {ex.Message}");
            return htmlContent;
        }
    }
}