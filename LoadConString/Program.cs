using Microsoft.Win32;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        string connectionString = "Data Source=localhost;Initial Catalog=BrokerMatePanorama3;Persist Security Info=True;User ID=sa;Password=Dreamer2161531@123";

        // Save the connection string to the Component Services in the Windows Registry
        using (RegistryKey key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Classes\\6049f4f0-0e5a-4b0b-930d-86911845a437\\PanoramaBackend.Api"))
        {
            key.SetValue("ConnectionString", connectionString);
            foreach (var item in Registry.LocalMachine.GetSubKeyNames())
            {
                System.Console.WriteLine(item);
            }
          
        }
        System.Console.WriteLine("Done");
        Console.ReadKey();
        }
}
