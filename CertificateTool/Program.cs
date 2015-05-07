using Newtonsoft.Json;
using Platform.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CertificateTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hi, welcome to the Certificate tool");
            Console.WriteLine("Enter the command you wish to execute: ");
            var cmd = Console.ReadLine();

            if (cmd.ToLower().Equals("test"))
            {
                Console.WriteLine("Are you testing a OneBox? (y|n):");
                var ans = Console.ReadLine();
                if (ans.ToLower().StartsWith("y"))
                {
                    Console.WriteLine("Is this a local OneBox? (y|n):");
                    var ans2 = Console.ReadLine();
                    if (ans2.ToLower().StartsWith("y"))
                    {
                        ClientKEKTest();
                        ServerKEKTest();
                    }
                    else
                    {
                        var Server = GetServerHost();
                        ClientKEKTest(Server: Server);
                    }
                }
                else
                {
                    var Server = GetServerHost();
                    
                    var webPort = "8443";
                    Console.WriteLine("Enter web port of Server service: (Default:{0})",webPort);
                    
                    var enterWebPort = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(enterWebPort))
                    {
                        webPort = enterWebPort;
                    }

                    var servicePort = "8808";
                    Console.WriteLine("Enter service port of Server service: (Default:{0})",servicePort);
                    var enterServicePort = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(enterServicePort))
                    {
                        servicePort = enterServicePort;
                    }

                    var environment = GetEnvironment();

                    ClientKEKTest(Server, environment, webPort, servicePort);
                }
            }
            else
            {
                Console.WriteLine("Wrong command entered, your choice of commmands are:");
                Console.WriteLine("test");
            }

            Console.WriteLine("Hit any key to continue");
            Console.ReadKey();
        }

        private static string GetServerHost()
        {
            Console.WriteLine("Enter fully qualified name of Server host:");
            var Server = Console.ReadLine();
            return Server;
        }

        private static string GetEnvironment()
        {
            Console.WriteLine("Enter environment name:");
            var environment = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(environment))
            {
                throw new InvalidOperationException("Environment name cannot be null or blank!");
            }
            return environment;
        }

        private const string ServerKEK = "ServerPrivateKeyContainer";

        //Assumes we're on Server box with ServerKEK container
        private static void ServerKEKTest(bool onServer=true)
        {
            //Get public key from container OR from web call
            string containerPubKey = null;
            if (onServer)
            {
                containerPubKey = PrintContainer(ServerKEK);
            }
            //Compare against DB, AccountSecurityAppliance table, PublicKey column
            //If mismatch, overwrite publickey column with container public key
        }

        private const string ClientKEK = "ClientPrivateKeyContainer";

        //Assumes we're on a OneBox
        //Will check if .pub file key matches the KEK container public key
        //This proves that we can send messages from Server and decrypt in FE
        private static void ClientKEKTest()
        {
            var environment = "ONEBOX";
            var webPort = "8443";
            var servicePort = "8808";
            ClientKEKTest(null,environment, webPort, servicePort);
        }

        //Assumes we're on an FE box with ClientKEK container
        private static void ClientKEKTest(string Server, string environment= "ONEBOX", string webPort= "8443", string servicePort = "8808")
        {
            var host = Server??"localhost";
            Console.WriteLine("Server Host: {0}", host);
            Console.WriteLine("Environment: {0}", environment);
            Console.WriteLine("Web Port: {0}", webPort);
            Console.WriteLine("Service port: {0}", servicePort);
            Console.WriteLine();
            var pubKeyFilePath =
                string.Format((Server==null?@"C:":@"\\"+host+@"\C$") + @"\docusign\{0}\Components\KeyMgr-{1}-{2}\Setup\rsa2048v1-1.pub", environment, webPort, servicePort);

            Console.WriteLine("Looking for public key file: {0}", pubKeyFilePath);
            if (!File.Exists(pubKeyFilePath))
            {
                Console.WriteLine("Error: File does not exist");
                return;
            }
            var filePubKey = GetPublicKeyFromFile(pubKeyFilePath);

            var container = ClientKEK;
            var containerPubKey = PrintContainer(container);
            Console.WriteLine("File public key: " + filePubKey);

            if (filePubKey.Equals(containerPubKey))
            {
                Console.WriteLine("Matches");
            }
            else
            {
                Console.WriteLine("Mismatch");

                Console.WriteLine();
                Console.WriteLine("Do you want to overwrite {0} with public key from container {1}? (y|n)");
                var ans = Console.ReadLine();
                if (ans.ToLower().Equals("y"))
                {
                    File.WriteAllText(pubKeyFilePath, JsonConvert.SerializeObject(Convert.FromBase64String(containerPubKey)));
                    Console.WriteLine("File updated");

                    //Restart the NT Service
                    var serviceName = string.Format("DSKeyMgr{0}",environment);
                    ServiceController sc = null;
                    if (Server == null)
                    {
                        sc = new ServiceController(serviceName);                        
                    }
                    else
                    {
                        sc = new ServiceController(serviceName, host);
                    }
                    Console.WriteLine("Restarting service {0} on host {1}", serviceName, host);
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                    sc.Start();
                }
            }
        }

        private static string PrintContainer(string container)
        {
            Console.Write("{0} Container ", container);
            if (!ContainerExists(container))
            {
                Console.WriteLine("does not exist. Cannot compare pub key file vs container (ERROR)");
            }
            else
            {
                Console.WriteLine("exists");
            }

            var containerPubKey = GetPublicKeyFromContainer(container);

            Console.WriteLine("Container public key: " + containerPubKey);
            return containerPubKey;
        }

        private static bool ContainerExists(string containerName)
        {
            return CngKey.Exists(containerName);
        }

        private static string GetPublicKeyFromContainer(string containerName)
        {
            if (!ContainerExists(containerName))
            {
                throw new InvalidOperationException("Container does not exist on machine: " + containerName);
            }

            var bytes = CryptoUtil.GetPublicForKeyExchangePair(KeyExchangeCryptoType.rsa2048v1, containerName);
            return Convert.ToBase64String(bytes);
        }

        private static string GetPublicKeyFromFile(string pubKeyFilePath)
        {
            //byte[] pubKey;
            var text = string.Empty;
            using (var sr = new StreamReader(pubKeyFilePath))
            {
                //The file contains quotes around the key value, so remove
                text = sr.ReadToEnd().Replace("\"", string.Empty);
                //pubKey = Convert.FromBase64String(text);
            }
            //return pubKey;
            return text;
        }
    }
}
