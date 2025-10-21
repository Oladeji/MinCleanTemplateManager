namespace MailSenderLib
{
    public class EmailConfig
    {
        public string EmailConfigType { get; set; } = "PLAIN";
        public string JsonValue { get; set; } = string.Empty;

    }


    public class EmailConfigValue
    {
        //  public string SenderName { get; set; } = string.Empty;
        //  public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = 0;
    }
}

