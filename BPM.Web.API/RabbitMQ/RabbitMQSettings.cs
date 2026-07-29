public class RabbitMQSettings
{
    public string Host { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string VirtualHost { get; set; }

    public bool UseSsl { get; set; }

    public string PasswordHistoryQueue { get; set; } = "password_history_queue";
    public string UserLoginHistoryQueue { get; set; } = "user_login_history_queue";
    public string RefreshTokenQueue { get; set; } = "user_refresh_token_queue";
}
