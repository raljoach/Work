using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyManagerTestTool
{
    class Program
    {
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
                    var sysConfig = AskSysConfigSettings();
                    

                    //This is the account to send envelopes
                    //Make sure this account has no external security appliance attached to it
                    var acct = AskForAccount();
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);
                    AskAccountSettings(acct);
                    AskMemberSettings(acct);

                    //Check to see 111-111-111 account is associated with a localhost keymanager
                    var keyMan = GetOneBoxMTSA();

                    SetSysConfig(true, environment, sysConfig);
                    RemoveExternalKMs(acct);
                    AssociateWithKeyManager(acct, keyMan);
                    SendEnvelope(acct);
                    QuerySentEnvelopes(acct);
                    LookForKazmonEvents(environment, keyMan);
                }
                else
                {
                    var sysConfig = AskSysConfigSettings();

                    //This is the account to send envelopes
                    //Make sure this account has no external security appliance attached to it
                    var acct = AskForAccount();
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);

                    //Get KeyManager Info for remote OneBox
                    var keyMan = AskKeyManInfo(KeyManType.Internal);
                    SetSysConfig(false, environment, sysConfig);
                    RemoveExternalKMs(acct);
                    AssociateWithKeyManager(acct, keyMan);
                    SendEnvelope(acct);
                    QuerySentEnvelopes(acct);
                    LookForKazmonEvents(environment, keyMan);
                }
            }
            else
            {
                Console.WriteLine("Enter environment:");
                environment = Console.ReadLine();

                Console.WriteLine("Is this an external Key Manager? (y|n)");
                ans = Console.ReadLine();

                if (ans.ToLower().StartsWith("y"))
                {
                    var keyMan = AskKeyManInfo(KeyManType.External);

                    var sysConfig = AskSysConfigSettings();

                    //This is the account to send envelopes
                    //Attach external security appliance to this email account
                    var acct = AskForAccount();
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);

                    SetSysConfig(false, environment, sysConfig);
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
                    var acct = AskForAccount();
                    AskEnableMartini(acct);
                    AskUseVdevApi(acct);

                    SetSysConfig(false, environment, sysConfig);
                    RemoveExternalKMs(acct);
                    AssociateWithKeyManager(acct, keyMan);
                    SendEnvelope(acct);
                    QuerySentEnvelopes(acct);
                    LookForKazmonEvents(environment, keyMan);
                }
            }
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
            throw new NotImplementedException();
        }

        private static void AskEnableMartini(AccountInfo acct)
        {
            throw new NotImplementedException();
        }

        private static void SendEnvelope(AccountInfo acct)
        {
            Console.WriteLine("Would you like to wait for your login session to timeout between initiating an envelope and actually sending it? (y|n)");
            var timeout = Console.ReadLine();

            Console.WriteLine("How many recipients do you want to add to envelope? (1+)?");
            var recipientCount = int.Parse(Console.ReadLine());



            throw new NotImplementedException();
        }

        private static void SetSysConfig(bool local, string environment, SysConfig sysConfig)
        {
            if (sysConfig == null)
            {
                Console.WriteLine("No sys config changes will be made");
            }
            else
            {

            }
            throw new NotImplementedException();
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
            Console.WriteLine("Enter KeyManager host address:");
            keyMan.Host = Console.ReadLine();

            keyMan.WebPort = type == KeyManType.Internal? "8443":"443";
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

            acct.AccountId = LookUpAccountId();

            return acct;
        }

        private static string LookUpAccountId()
        {
            throw new NotImplementedException();
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
