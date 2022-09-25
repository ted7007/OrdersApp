namespace InternalService.Config;

public class MySQLOptions
{
    public const string OptionName = "MySQLOptions";
    
    public string Server { get; set; }
    
    public string Port { get; set; }

    public string UserName { get; set; }

    public string DatabaseName { get; set; }
    
    public string Password { get; set; }
}