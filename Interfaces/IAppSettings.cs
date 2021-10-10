namespace Interfaces
{
    public interface IAppSettings
    {
        public string Databasename { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string QueueName { get; set; }
    }
}
