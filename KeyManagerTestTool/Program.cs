using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KeyManagerTestTool
{
    class Program
    {
        private static Topology topology;
        private static string oneBoxHost = "localhost";
        private static RestApi restApi = null;
        private static string integratorKey = null;
        private static string dbHost = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Hi, welcome to the KeyManager test tool!");
            /*Console.WriteLine("Is this a perf/stress test?");
            Console.WriteLine("How long should I run the stress test for?");
            Console.WriteLine("How many iterations should I run in the stress test?");
            Console.WriteLine("How many threads should I run for stress test?");
            Console.WriteLine("Should I perf test via front-end or back-end? (FE|BE)?");
            Console.WriteLine("Shoudl I perf test via UI or API? (API|UI)");*/

            Console.WriteLine("Is this a OneBox? (y|n)");
            var environment = "ONEBOX";
            var ans = Console.ReadLine();
            
            if (ans.ToLower().StartsWith("y"))
            {
                Console.WriteLine("Is this a local OneBox? (y|n)");
                ans = Console.ReadLine();
                if (ans.ToLower().StartsWith("y"))
                {
                    topology = Topology.OneBoxLocal;
                    var sysConfig = AskSysConfigSettings();

                    //This is the account to send envelopes
                    //Make sure this account has no external security appliance attached to it                    
                    var acct = AskForAccount();
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);
                    AskAccountSettings(acct);
                    AskMemberSettings(acct);
                    SetSysConfig(environment, sysConfig);

                    //Check to see 111-111-111 account is associated with a localhost keymanager
                    var keyMan = GetOneBoxMTSA();                    
                    RemoveExternalKMs(acct);
                    AssociateWithKeyManager(acct, keyMan);
                    SendEnvelope(acct);
                    QuerySentEnvelopes(acct);
                    LookForKazmonEvents(environment, keyMan);
                }
                else
                {
                    topology = Topology.OneBoxRemote;
                    Console.WriteLine("Enter address of remote OneBox host:");
                    oneBoxHost = Console.ReadLine();
                    
                    var sysConfig = AskSysConfigSettings();

                    //This is the account to send envelopes
                    //Make sure this account has no external security appliance attached to it
                    var acct = AskForAccount();
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);
                    SetSysConfig(environment, sysConfig);

                    //Get KeyManager Info for remote OneBox
                    var keyMan = AskKeyManInfo(KeyManType.Internal);                    
                    RemoveExternalKMs(acct);
                    AssociateWithKeyManager(acct, keyMan);
                    SendEnvelope(acct);
                    QuerySentEnvelopes(acct);
                    LookForKazmonEvents(environment, keyMan);
                }
            }
            else
            {
                topology = Topology.Distributed;
                Console.WriteLine("Enter environment:");
                environment = Console.ReadLine();
                var acct = AskForAccount();

                Console.WriteLine("Is this an external Key Manager? (y|n)");
                ans = Console.ReadLine();

                if (ans.ToLower().StartsWith("y"))
                {
                    var keyMan = AskKeyManInfo(KeyManType.External);

                    var sysConfig = AskSysConfigSettings();

                    //This is the account to send envelopes
                    //Attach external security appliance to this email account                    
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);
                    SetSysConfig(environment, sysConfig);

                    RemoveExternalKMs(acct);
                    AssociateWithKeyManager(acct, keyMan);
                    SendEnvelope(acct);
                    QuerySentEnvelopes(acct);
                    LookForKazmonEvents(environment,keyMan);
                }
                else
                {
                    //This will be Test/Stage/Demo
                    var keyMan = AskKeyManInfo(KeyManType.Internal);
                    var sysConfig = AskSysConfigSettings();
                    //This is the account to send envelopes
                    //Make sure this account has no external security appliance attached to it
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);

                    SetSysConfig(environment, sysConfig);
                    RemoveExternalKMs(acct);
                    AssociateWithKeyManager(acct, keyMan);
                    SendEnvelope(acct);
                    QuerySentEnvelopes(acct);
                    LookForKazmonEvents(environment, keyMan);
                }
            }
        }

        private static RestApi GetRestApi()
        {
            if(restApi==null)
            {
                var protocol = "http";
                Console.WriteLine("Enter protocol for REST API service: ({0})",protocol);
                var enter = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enter))
                {
                    protocol = enter;
                }

                var host = (topology==Topology.OneBoxLocal || topology == Topology.OneBoxRemote)?oneBoxHost:null;
                if (string.IsNullOrWhiteSpace(host))
                {
                    Console.WriteLine("Enter host address for REST API service: ({0})", host);
                    enter = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(enter))
                    {
                        host = enter;
                    }
                }

                var version = "v2";
                Console.WriteLine("Enter version of REST API to use: ({0})", version);
                enter = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enter))
                {
                    version = enter;
                }

                restApi = new RestApi(protocol, host, version);
            }
            return restApi;
        }

        private static void AskMemberSettings(AccountInfo acct)
        {
            throw new NotImplementedException();
        }

        private static void AskAccountSettings(AccountInfo acct)
        {
            bool noSettingsFound = true;
            if (noSettingsFound)
            {
                RunBigHammer();
            }
            throw new NotImplementedException();
        }

        private static void RunBigHammer()
        {
            throw new NotImplementedException();
        }

        private static void QuerySentEnvelopes(AccountInfo acct)
        {
            throw new NotImplementedException();
        }

        private static void LookForKazmonEvents(string environment, KeyManagerInfo keyMan)
        {
            Console.WriteLine(@"Shall I look for SA Internal\Call exceptions?");
            Console.WriteLine(@"Shall I look for SA Internal\Dual Mode Mismatch errors?");
            Console.WriteLine(@"Shall I look for Blob\Call exceptions?");
            Console.WriteLine(@"Shall I look for Exception.Report errors with envelope id?");
            Console.WriteLine(@"Shall I run a hadoop query on tokencodes within an exceptions?");
            Console.WriteLine(@"Shall I look for API\Request errors with envelope id?");
            Console.WriteLine(@"Shall I check Kazmon queue is backed up, and thus why we may not be getting events we expect?");
            Console.WriteLine(@"Shall I query the API Error Log Report for errors?");
            throw new NotImplementedException();
        }

        private static void AskUseVdevApi(AccountInfo acct)
        {
            //throw new NotImplementedException();
        }

        private static void AskEnableMartini(AccountInfo acct)
        {
            var ans = "n";
            Console.WriteLine("Would you like to enable Martini for account {0}? (y|n) [Default:n]", acct.Email);
            var enter = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(enter))
            {
                ans = enter;
            }

            if (ans.ToLower().StartsWith("y"))
            {
                Console.WriteLine("Sorry, I don't know how to enable Martini for an account");
            }
        }

        private static void SendEnvelope(AccountInfo acct)
        {
            Console.WriteLine("Would you like to wait for your login session to timeout between initiating an envelope and actually sending it? (y|n)");
            var timeout = Console.ReadLine();

            Console.WriteLine("How many recipients do you want to add to envelope? (1+)?");
            var recipientCount = int.Parse(Console.ReadLine());

            throw new NotImplementedException();
        }

        private static void SetSysConfig(string environment, SysConfig sysConfig)
        {
            if (sysConfig == null)
            {
                Console.WriteLine("No sys config changes will be made");
            }
            else
            {
                Console.WriteLine("Sorry, I don't know how to make sys config changes");
            }
        }

        private static void RemoveExternalKMs(AccountInfo acct)
        {
            throw new NotImplementedException();
        }

        private static void AssociateWithKeyManager(AccountInfo acct, KeyManagerInfo keyMan)
        {
            if (keyMan.Type == KeyManType.External)
            {
                //check to see if there are 0 externalkey managers

                //bool exKMCount = 0;
                //if (exKMCount == 0)
                //{
                    EnableMemberSecurityAppliance(acct);
                    AddMemberSecurityAppliance(acct, keyMan);
                //}
            }
            throw new NotImplementedException();
        }

        private static void AddMemberSecurityAppliance(AccountInfo acct, KeyManagerInfo keyMan)
        {
            throw new NotImplementedException();
        }

        private static void EnableMemberSecurityAppliance(AccountInfo acct)
        {
            throw new NotImplementedException();
        }        

        

        private static KeyManagerInfo GetOneBoxMTSA()
        {
            return new KeyManagerInfo(KeyManType.Internal) { Host = "localhost", ServicePort = "8808", WebPort = "8443" };
        }

        private static KeyManagerInfo AskKeyManInfo(KeyManType type = KeyManType.Internal)
        {
            KeyManagerInfo keyMan = new KeyManagerInfo(type);
            var knownKeyManLocation = type == KeyManType.Internal && (topology == Topology.OneBoxLocal || topology == Topology.OneBoxRemote);
            if (knownKeyManLocation)
            {
                keyMan.Host = oneBoxHost;
            }
            else
            {
                Console.WriteLine("Enter KeyManager host address:");
                keyMan.Host = Console.ReadLine();
            }

            if (!knownKeyManLocation)
            {
                keyMan.WebPort = type == KeyManType.Internal ? "8443" : "443";
                Console.WriteLine("Enter KeyManager web port: ({0})", keyMan.WebPort);
                var enter = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enter))
                {
                    keyMan.WebPort = enter;
                }

                keyMan.ServicePort = type == KeyManType.Internal ? "8808" : "8008";
                Console.WriteLine("Enter KeyManager service port: ({0})", keyMan.ServicePort);
                enter = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(enter))
                {
                    keyMan.ServicePort = enter;
                }
            }
            return keyMan;
        }

        private static AccountInfo AskForAccount()
        {
            AccountInfo acct = new AccountInfo();
            acct.Email = "dev.user@docusign.com";
            Console.WriteLine("Enter account email address: ({0})", acct.Email);
            var enter = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(enter))
            {
                acct.Email = enter;
            }

            acct.Password = "docusign";
            Console.WriteLine("Enter account password: ({0})", acct.Password);
            enter = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(enter))
            {
                acct.Password = enter;
            }

            LookUpAccountId(acct);

            return acct;
        }

        private static void LookUpAccountId(AccountInfo acct)
        {
            RequestInfo req = new RequestInfo()
            {
                Uri = GetRestApi().AccountUrl(AccountAction.Login_Information),
                ContentType = ContentTypes.Application_json,
                Headers = new Dictionary<string, string>() { 
                    { "X-DocuSign-Authentication", CreateAuthHeader(acct, FetchIntegratorKey()) },
                    {"X-DocuSign-AppToken", "platform_qa_apiToken"}
                }
            };
            var loginInfo = JObject.Parse(Http.Get(req));
            acct.AccountId = loginInfo.SelectToken("loginAccounts[0].accountId").ToString();
        }

        private static string FetchIntegratorKey()
        {
            if (integratorKey == null)
            {
                string connect = @"Server=" + GetDbHost() + ";Database=Docusign;Integrated Security=True";
                SqlConnection scn = new SqlConnection(connect);
                string getIKSQL = "SELECT TOP 1 [IntegratorKey] FROM [Docusign].[dbo].[IntegratorKey] WHERE [Enabled] = 1";
                SqlCommand IKQuery = new SqlCommand(getIKSQL, scn);
                scn.Open();
                string IK = (string)IKQuery.ExecuteScalar();
                if (IK == null)
                {
                    IK = AddIntegratorKey(scn);
                }
                scn.Close();
                integratorKey = IK;
            }
            return integratorKey;
        }

        private static string AddIntegratorKey(SqlConnection scn, string integratorName = "KeyManTestToolIK")
        {
            string newIK = Guid.NewGuid().ToString("D").ToUpper();
            SqlCommand spcmd = new SqlCommand("InsertIntegratorKey", scn);
            spcmd.CommandType = System.Data.CommandType.StoredProcedure;
            SqlParameter IntegratorName = new SqlParameter("@IntegratorName", integratorName);
            SqlParameter IntegratorKey = new SqlParameter("@IntegratorKey", newIK);
            SqlParameter Enabled = new SqlParameter("@Enabled", true);
            SqlParameter ClientFilterXml = new SqlParameter("@ClientFilterXml", "<clientFilters autoEnableNew=\"0\" autoAddNew=\"0\"><locations></locations></clientFilters>");
            spcmd.Parameters.Add(IntegratorName);
            spcmd.Parameters.Add(IntegratorKey);
            spcmd.Parameters.Add(Enabled);
            spcmd.Parameters.Add(ClientFilterXml);
            int result = spcmd.ExecuteNonQuery();
            if (result == 1) { return newIK; }
            else
            {
                throw new Exception("unable to execute InsertIntegratorKey sproc.");
            }
        }

        private static string GetDbHost()
        {
            if (dbHost == null)
            {
                if (topology == Topology.OneBoxLocal || topology == Topology.OneBoxRemote)
                {
                    dbHost = string.Format(@"{0}\DS02",oneBoxHost);
                }
                Console.WriteLine("Enter DB host: {0}", dbHost==null?string.Empty:string.Format("({0})",dbHost));
                var enter = Console.ReadLine();
                if (dbHost == null || !string.IsNullOrWhiteSpace(enter))
                {
                    dbHost = enter;
                }
            }
            return dbHost;
        }

        private static string CreateAuthHeader(AccountInfo acct, string integratorKey)
        {
            var authHeader = string.Format(
                @"
                  <DocuSignCredentials>
                    <Username>{0}</Username>
                    <Password>{1}</Password>
                    <IntegratorKey>{2}</IntegratorKey>
                  </DocuSignCredentials>",
                 acct.Email, acct.Password, integratorKey);
            return authHeader;
        }

        private static SysConfig AskSysConfigSettings()
        {
            Console.WriteLine("Would you like to modify the current sys config values? (y|n)");
            var mod = Console.ReadLine();

            if (mod.ToLower().StartsWith("y"))
            {
                SysConfig sysConfig = new SysConfig();
                Console.WriteLine("Enter Blobs.EnvelopeDetailsRamp value: (0-100)");
                sysConfig.EnvDetailsRamp = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter Blobs.EnvelopeDetailReadFirst value: (BLOB|DUAL|LEGACY)");
                sysConfig.EnvDetailsReadFirst = Console.ReadLine();

                Console.WriteLine("Enter Blobs.EnvelopeDetailWrites value: (DUAL)");
                sysConfig.EnvDetailsWrites = Console.ReadLine();

                Console.WriteLine("Enter KeyManagerDualMode value: (8000-90)");
                sysConfig.KeyManagerDualMode = Console.ReadLine();

                Console.WriteLine("Enter KeyManagerRemoteMode value: (8000-50)");
                sysConfig.KeyManagerRemoteMode = Console.ReadLine();

                return sysConfig;
            }
            return null;
        }
    }
}
