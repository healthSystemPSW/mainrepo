namespace Integration.Shared.Model
{
    class EmailConfiguration
    {
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public bool EnableSSL { get; set; }
    }
}
