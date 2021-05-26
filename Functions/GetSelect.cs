using Newtonsoft.Json;
using SellCards;
using SteamBot.SteamWebBot.Account;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RemoveItemsMarket.Functions {
    class GetSelect {
        public static void GET(SteamWebBotAccount account, List<string> urls) {

            List<string> ids = new List<string>();

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

        public class Root {
            public bool success { get; set; }
            public int total_count { get; set; }
            public string results_html { get; set; }
        }
    }
}
