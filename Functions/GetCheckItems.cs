using Newtonsoft.Json;
using SellCards;
using SteamBot.SteamWebBot.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RemoveItemsMarket.Functions {
    class GetCheckItems {
        public static void Get(SteamWebBotAccount account) {

            string URL = $"https://steamcommunity.com/market/mylistings?count=10";
            var responseCount = new RequestBuilder(URL).GET()
               .AddCookies(account.SteamGuard)
               .Execute();

            Root qtdItems = JsonConvert.DeserializeObject<Root>(responseCount.Content);
            Logger.info($"Items total: {qtdItems.total_count}");

            if (qtdItems.total_count > 0) {

                List<string> urls = new List<string>();
                List<string> ids = new List<string>();

                int qtd = qtdItems.total_count / 100;

                for (int i = 0; i < qtd; i++) {
                    int iPlus = i + 1;

                    urls.Add($"https://steamcommunity.com/market/mylistings/render/?query=&start={i}00&count={iPlus}00");
                }
                foreach (var url in urls) {

                    var responseItem = new RequestBuilder(url).GET()
                    .AddCookies(account.SteamGuard)
                    .Execute();

                    Root qtdDeseralize = JsonConvert.DeserializeObject<Root>(responseItem.Content);

                    var cards = qtdDeseralize.results_html.Split("market_recent_listing_row");

                    foreach (string card in cards) {
                        if (card.Contains("mylisting_")) {

                            int inicio = card.IndexOf("mylisting_");
                            int fim = card.IndexOf("\"", inicio);
                            string postid = card.Substring(inicio, fim - inicio);
                            string postIdRemove = String.Join("", Regex.Split(postid, @"[^\d]"));

                            ids.Add(postIdRemove);
                        }
                    }
                }
                GetRemoveItems.Get(account, ids);
            }
        }

        public class Root {
            public bool success { get; set; }
            public int total_count { get; set; }
            public string results_html { get; set; }
        }
    }
}
