using RemoveItemsMarket.Functions;
using SellCards;
using SellCards.Models;
using SteamBot.SteamWebBot.Account;
using System;
using System.Collections.Generic;
using System.IO;

namespace RemoveItemsMarket {
    class Program {

        public static string login = "";
        public static string password = "";

        public static Dictionary<string, MafileProcessingModel> allMafiles = MafilesProcessing.GetAllMafiles();
        public static string[] allAccounts = File.ReadAllLines(@"Config\Accs.txt");
        static void Main(string[] args) {
            Console.Title = $"RemoveItems -- AccountsLoad: {allAccounts.Length} -- MaFilesLoad: {allMafiles.Count}";

            var counter = 0;
            foreach (var acc in allAccounts) {
                try {
                    var accSpl = acc.Split(':');
                    login = accSpl[0].ToLower();
                    password = accSpl[1];

                    Logger.info("Processing {0}. {1}/{2}", login, ++counter, allAccounts.Length);
                    if (!allMafiles.ContainsKey(login)) {
                        Logger.error(login + " mafile not found");
                        continue;
                    }

                    var account = new SteamWebBotAccount(login, password, allMafiles[login]);

                    GetCheckItems.Get(account);

                } catch (Exception e) {
                    Logger.error($"Error: {e.ToString()}");
                }
            }
            Logger.info("All Done");
            Console.ReadKey();
        }
    }
}
