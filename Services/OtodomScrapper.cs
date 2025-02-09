
using System.Linq;
using HtmlAgilityPack;
using Services.Interfaces;
using HtmlAgilityPack;
using ShittyApi.Models;
using System.Text.RegularExpressions;

namespace Services
{
    public interface IOtodomScrapper : IScrapper{};
    public class OtodomScrapper : IOtodomScrapper
    {
        public string SearchUrl { get; set; }
        string searchUrl = "https://www.otodom.pl/pl/wyniki/wynajem/mieszkanie/mazowieckie/";
        string baseUrl = "https://www.otodom.pl";
        string city = "warszawa?";
        byte[] rawData = new byte[] { };
        List<OtodomDataModel> results = new List<OtodomDataModel>();
        public OtodomScrapper() 
        { 
            SearchUrl = searchUrl + city;
        }

        public IScrapper AddParameter<T>(string param, T value)
        {
            SearchUrl += $"{param}={value}&";
            return this;
        }

        public IScrapper BuildParameters()
        {
            throw new NotImplementedException();
        }

        public object GetFormatedData()
        {
            return results;
        }

        public byte[] GetScrappedData()
        {
            return rawData;
        }

        public async Task<IScrapper> Scrap()
        {
            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders
                .Add(
                    "User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36"
                );
                var response = await client.GetAsync(SearchUrl);

                response.EnsureSuccessStatusCode();

                rawData = await response.Content.ReadAsByteArrayAsync();
            }
            await ParseData();
            return this;
        }

        private async Task<bool> ParseData()
        {
            string markup = System.Text.Encoding.UTF8.GetString(rawData);
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(markup);

            var nodes = htmlDoc.DocumentNode
                                .Descendants("article")
                                .Where(node => node.GetAttributeValue("data-cy", "")
            .Contains("listing-item"));
            foreach(var node in nodes)
            {
                string title = node.Descendants("p")
                    .First(node => node.GetAttributeValue("data-cy", "") == "listing-item-title")
                    .InnerText;
                string url = node.Descendants("a")
                    .First(node => node.GetAttributeValue("data-cy", "") == "listing-item-link")
                    .Attributes["href"].Value;

                Decimal price = 0.00m;
                string currency = string.Empty;
                string priceString = node.Descendants("span")
                    .First(node => node.GetAttributeValue("direction", "") == "horizontal")
                    .InnerText;

                ParsePrice(ref price, ref currency, priceString);

                results.Add(new OtodomDataModel()
                {
                    Name = title,
                    Url = baseUrl + url,
                    Price = price,
                    Currency = currency
                });
            }

            return true;
        }

        private static void ParsePrice(ref decimal price, ref string currency, string priceString)
        {
            string pattern = @"(\d+)\s*([A-Za-zł\$€£]+)";
            Match match = Regex.Match(priceString, pattern);
            if (match.Success)
            {
                price = Decimal.Parse(match.Groups[1].Value);
                currency = match.Groups[2].Value;
            }
        }
    }
}