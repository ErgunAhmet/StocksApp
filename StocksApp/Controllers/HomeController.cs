using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StocksApp.Models;
using StocksApp.Services;

namespace StocksApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly FinnhubService _finnhubService;
		private readonly IConfiguration _configuration;
		private readonly IOptions<TradingOptions> _tradingoptions;

		public HomeController(FinnhubService finnhubService,
			IConfiguration configuration,
			IOptions<TradingOptions> tradingOptions)
		{
			_finnhubService = finnhubService;
			_configuration = configuration;
			_tradingoptions = tradingOptions;
		}

		[Route("/")]
		public async Task<IActionResult> Index()
		{
			if (_tradingoptions.Value.DefaultStockSymbol== null)
			{
				_tradingoptions.Value.DefaultStockSymbol = "MSFT";
			}
			Dictionary<string, object> responseDictionary =
				await _finnhubService.GetStockPriceQuote
				(_tradingoptions.Value.DefaultStockSymbol);

			Stock stock = new Stock()
			{
				StockSymbol = _tradingoptions.Value.DefaultStockSymbol,
				CurrentPrice = Convert.ToDouble(responseDictionary["c"].ToString()),
				HighestPrice = Convert.ToDouble(responseDictionary["h"].ToString()),
				LowestPrice= Convert.ToDouble(responseDictionary["l"].ToString()),
				OpenPrice= Convert.ToDouble(responseDictionary["o"].ToString()),

			};
			return View(stock);
		}
	}
}
