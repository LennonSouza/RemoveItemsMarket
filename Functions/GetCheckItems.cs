using Newtonsoft.Json;
using SellCards;
using SteamBot.SteamWebBot.Account;
using System.Collections.Generic;

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

                if (qtdItems.total_count < 100) {

                    urls.Add($"https://steamcommunity.com/market/mylistings/render/?query=&start=0&count=100");

                    GetSelect.GET(account, urls);

                } else if (qtdItems.total_count > 100) {

                    int qtd = qtdItems.total_count / 100;

                    for (int i = 0; i < qtd; i++) {
                        int iPlus = i + 1;

                        urls.Add($"https://steamcommunity.com/market/mylistings/render/?query=&start={i}00&count={iPlus}00");
                    }
                    GetSelect.GET(account, urls);
                }
            }
        }

        public class Root {
            public bool success { get; set; }
            public int total_count { get; set; }
            public string results_html { get; set; }
        }
    }
}
