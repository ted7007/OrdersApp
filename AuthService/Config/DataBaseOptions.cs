﻿namespace InternalService.Config;

public class DataBaseOptions
{
    public const string OptionName = "DBOptions";
    
    public string Server { get; set; }
    
    public string Port { get; set; }

    public string UserName { get; set; }

    public string DatabaseName { get; set; }
    
    public string Password { get; set; }
}