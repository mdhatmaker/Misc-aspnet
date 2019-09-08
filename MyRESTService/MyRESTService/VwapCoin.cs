using System;
using System.Linq;
using System.Collections.Generic;
using CryptoTools;
using CryptoTools.General;
using ExchangeSharp;
using System.Threading.Tasks;
using System.Text;

namespace MyRESTService
{
    public static class VwapCoin
    {
        // ideas:
        // 1. for each exchange, weight its VWAP by the volume that traded during the "window"
        // 2. 

        static Dictionary<string, IExchangeAPI> m_apiMap;
        static Credentials m_creds;
        
		public static string Calculate(string[] args, string globalSymbol)
        {
            args = "bitfinex BTCUSDT 5 /Users/michael/Documents/hat_apis.csv.enc mywookie".Split(' ');

            // Pass TWO arguments: encrypted API key/secret filename AND 8-char password
            if (args.Length < 5)
            {
                Console.WriteLine("usage: dotnet vwap.dll <exchange> <symbol> <minutes> <encrypted_api_file> <8_char_password>");
                Console.WriteLine("\n   ex: dotnet vwap.dll binance BTCUSDT 60 /Users/david/apis.csv.enc A5s78rQz\n");
                return null;
            }


            m_creds = Credentials.LoadEncryptedCsv(args[3], args[4]);
            //string k = "BINANCE";
            //Console.WriteLine("{0},{1},{2},{3}", k, creds[k].Key, creds[k].Secret, creds[k].Passphrase);

            string exchange = args[0].ToUpper();            // "BINANCE", "KRAKEN", "GDAX", ...

            //var symbol = "BTCUSDT";
            //int minutes = 60;
            string symbol = args[1];
            int minutes = int.Parse(args[2]);

            // ---------- Execute methods for various exchanges ----------------------------------------
            /*var bina = new Binance(m_creds);
            bina.BinanceVwap(symbol, DateTime.Now.ToUniversalTime(), minutes);

            var bitt = new Bittrex(m_creds);
            bitt.BittrexSymbols();
            bitt.BittrexVwap(symbol, DateTime.Now.ToUniversalTime(), minutes);

            var gdax = new Gdax(m_creds);
            gdax.GdaxVwap(symbol, DateTime.Now.ToUniversalTime(), minutes);

            var bitf = new BitFinex(m_creds);
            bitf.BitfinexVwap(symbol, DateTime.Now.ToUniversalTime(), minutes);*/
            //------------------------------------------------------------------------------------------

            // *** ExchangeSharp ***
            // https://github.com/jjxtra/ExchangeSharp
            //IExchangeAPI api;
            //ExchangeTicker ticker;
            //IEnumerable<ExchangeTrade> trades;

            string vwaps = VwapForPrimaryExchanges(globalSymbol);
			return vwaps;

            /*VwapForAllExchanges("BTC-USD");
            VwapForAllExchanges("BTC-USDT");
            VwapForAllExchanges("ETH-USD");
            VwapForAllExchanges("ETH-USDT");
            //VwapForAllExchanges("BTC-ETH");
            //Console.WriteLine();*/

            //TickerForAllExchanges("BTC-USD");
            //TickerForAllExchanges("BTC-USDT");
            //TickerForAllExchanges("ETH-BTC");

            //var sinceDateTime = trades.First().Timestamp;
            //api.GetHistoricalTrades(callback, symbol, sinceDateTime);

            //Console.WriteLine("\n(end of Main)");
            //Console.ReadKey();
        }

        static bool callback(IEnumerable<ExchangeTrade> trades)
        {
            trades.ToList().ForEach(t => t.Print());
            return true;
        }

        // Extension Method: Display contents of an ExchangeTrade object
        public static void Print(this ExchangeTrade t)
        {
            Console.WriteLine("{0} {1} {2}", t.Timestamp.ToDisplay(), t.Price, t.Amount);
        }

        static IDictionary<string, IExchangeAPI> GetPrimaryExchangeApis()
        {

            if (m_apiMap == null)
            {
                m_apiMap = new Dictionary<string, IExchangeAPI>();

                m_apiMap.Add("Kraken", new ExchangeKrakenAPI());
                //m_apiMap.Add("Gemini", new ExchangeGeminiAPI());
                //m_apiMap.Add("Bittrex", new ExchangeBittrexAPI());
                m_apiMap.Add("Gdax", new ExchangeGdaxAPI());
                //m_apiMap.Add("Bithumb", new ExchangeBithumbAPI());    // TODO: What's wrong with Bithumb?
                m_apiMap.Add("Bitfinex", new ExchangeBitfinexAPI());
                m_apiMap.Add("Bitstamp", new ExchangeBitstampAPI());
                //m_apiMap.Add("Poloniex", new ExchangePoloniexAPI());
            }
            return m_apiMap;
        }

        static IDictionary<string, IExchangeAPI> GetAllExchangeApis()
        {

            if (m_apiMap == null)
            {
                m_apiMap = new Dictionary<string, IExchangeAPI>();

                m_apiMap.Add("Kraken", new ExchangeKrakenAPI());
                m_apiMap.Add("Gemini", new ExchangeGeminiAPI());
                m_apiMap.Add("Abucoins", new ExchangeAbucoinsAPI());
                m_apiMap.Add("Hitbtc", new ExchangeHitbtcAPI());
                m_apiMap.Add("Bittrex", new ExchangeBittrexAPI());
                m_apiMap.Add("Binance", new ExchangeBinanceAPI());
                m_apiMap.Add("Okex", new ExchangeOkexAPI());
                m_apiMap.Add("Huobi", new ExchangeHuobiAPI());
                m_apiMap.Add("Yobit", new ExchangeYobitAPI());
                m_apiMap.Add("Gdax", new ExchangeGdaxAPI());
                m_apiMap.Add("Kucoin", new ExchangeKucoinAPI());
                //m_apiMap.Add("Bithumb", new ExchangeBithumbAPI());    // TODO: What's wrong with Bithumb?
                m_apiMap.Add("Bitfinex", new ExchangeBitfinexAPI());
                m_apiMap.Add("Bitstamp", new ExchangeBitstampAPI());
                m_apiMap.Add("Livecoin", new ExchangeLivecoinAPI());
                m_apiMap.Add("Poloniex", new ExchangePoloniexAPI());
                m_apiMap.Add("Bleutrade", new ExchangeBleutradeAPI());
                m_apiMap.Add("Cryptopia", new ExchangeCryptopiaAPI());
                m_apiMap.Add("Tux", new ExchangeTuxExchangeAPI());
            }
            return m_apiMap;
        }

        static void VwapForAllExchanges(string global_symbol)
        {
            Console.WriteLine();
            var apiMap = GetAllExchangeApis();
            foreach (var kv in apiMap)
            {
                var exchange = kv.Key;
                var api = kv.Value;
                Vwap(api, exchange, global_symbol);
            }
        }

        static string VwapForPrimaryExchanges(string global_symbol)
        {
			//Console.WriteLine();
			StringBuilder sb = new StringBuilder();
            var apiMap = GetPrimaryExchangeApis();
            foreach (var kv in apiMap)
            {
                var exchange = kv.Key;
                var api = kv.Value;
                var vwap = Vwap(api, exchange, global_symbol);
				sb.Append(vwap);
            }
			return sb.ToString();
        }
        
        static string Vwap(IExchangeAPI api, string exchange, string global_symbol, bool displayTrades = false)
        {
            try
            {
                var symbol = api.GlobalSymbolToExchangeSymbol(global_symbol);       // "BTC-KRW" for Bithumb?
                var trades = api.GetRecentTrades(symbol);
                if (displayTrades) trades.ToList().ForEach(t => t.Print());
                int tradeCount = trades.Count();
                if (tradeCount == 0)
                {
					return $"Zero trades returned for [{exchange} {symbol}].\n";
                }
                var sumPQ = trades.Sum(t => t.Price * t.Amount);
                var sumQ = trades.Sum(t => t.Amount);
                var vwap = sumPQ / sumQ;
                var sortedTrades = trades.OrderBy(t => t.Timestamp);
                var firstTime = sortedTrades.First().Timestamp;
                var lastTime = sortedTrades.Last().Timestamp;
                var minutes = lastTime.Subtract(firstTime).TotalMinutes;
                return string.Format("[{0} {1}] ({2} trades) VWAP = {3:0.00000000}           first:{4}  last:{5}  ({6:0.0} minutes)\n", exchange, symbol, tradeCount, vwap, firstTime, lastTime, minutes);
            }
            catch (Exception ex)
            {
                return string.Format("***[{0} {1}] ERROR: {2}***\n", exchange, global_symbol, ex.Message);
            }
        }

        static void TickerForAllExchanges(string global_symbol)
        {
            List<Task> taskList = new List<Task>();

            Console.WriteLine();
            var apiMap = GetAllExchangeApis();
            foreach (var kv in apiMap)
            {
                var exchange = kv.Key;
                var api = kv.Value;
                //Task t = new Task(function);
                //t.Start();
                var t = Ticker(api, exchange, global_symbol);
                taskList.Add(t);
            }

            Task.WaitAll(taskList.ToArray());
        }

        static async Task Ticker(IExchangeAPI api, string exchange, string global_symbol)
        {
            try
            {
                var symbol = api.GlobalSymbolToExchangeSymbol(global_symbol);
                var ticker = await api.GetTickerAsync(symbol);
                //var ticker = api.GetTicker(symbol);
                Console.WriteLine("[{0} {1}] b:{2} a:{3}", exchange, symbol, ticker.Bid, ticker.Ask);
            }
            catch (Exception ex)
            {
                //Console.WriteLine("***[{0} {1}] ERROR: {2}***", exchange, global_symbol, ""); //ex.Message);
            }
        }


    } // end of class Program
} // end of namespace

