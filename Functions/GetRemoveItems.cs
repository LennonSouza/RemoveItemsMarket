using SellCards;
using SteamBot.SteamWebBot.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RemoveItemsMarket.Functions {
    class GetRemoveItems {
        public static void Get(SteamWebBotAccount account, List<string> ids) {

            int count = 1;
            try {
                foreach (string id in ids) {
                    string requestRemove = $"https://steamcommunity.com/market/removelisting/{id}";

                    var restFinish = new RequestBuilder(requestRemove).POST()
                              .AddHeader(HttpRequestHeader.Referer, "https://steamcommunity.com/market/")
                              .AddPOSTParam("sessionid", account.SteamGuard.Session.SessionID)
                              .AddCookies(account.SteamGuard)
                              .Execute();

                    if (restFinish.StatusCode == HttpStatusCode.OK) {
                        Logger.info($"{count} have already been removed");
                        count++;
                    }
                }
            } catch (Exception e) {

                Logger.error($"Error: {e.ToString()}");
            }
            Logger.info($"Total items removed: {count}");
        }
    }
}
