using crawler.Context;
using crawler.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace crawler.Controllers
{
	public class HomeController : Controller
	{
		private readonly DatabaseContext _db;


		public HomeController(DatabaseContext db)
		{
	
			_db = db;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}
		public void Search(string q)
		{
			
			StartCrowlerAsync(q);

		}
		public async Task StartCrowlerAsync(string q)
		{
		

			q = q.ToLower();
			var url = "https://www.arabam.com/ikinci-el?searchText=" + q;
			var httpClient = new HttpClient();
			var html = await httpClient.GetStringAsync(url);

			var htmlDocument = new HtmlDocument();

			htmlDocument.LoadHtml(html);
			var tr = htmlDocument.DocumentNode.Descendants("tr").Where(node => node.GetAttributeValue("class", "").Contains("listing-list-item")).ToList();

			foreach (var item in tr)
			{
				SportCar car = new SportCar();
				car.Title = item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("listing-modelname pr")).FirstOrDefault().InnerText;

				car.Year = Convert.ToInt32(item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("listing-text pl8 pr8 tac pr")).FirstOrDefault().InnerText);

				var priceTd = item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("pl8 pr8 tac pr")).FirstOrDefault().InnerText;

				priceTd = priceTd.Substring(0, priceTd.Length - 4);

				car.Price = Convert.ToDecimal(priceTd);

				var locationTd = item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("listing-text pl8 pr8 tac pr")).LastOrDefault();
				car.Location = locationTd.Descendants("div").Where(p => p.GetAttributeValue("class", "").Equals(" fade-out-content-wrapper")).FirstOrDefault().InnerText;
				//carPrice = carPrice.Replace("  ", "");

				car.Id = 1;
				_db.SportCars.Add(car);
				_db.SaveChanges();


				//carPrice = carPrice.Replace("\n", "");
			}
		}


		
	}
}
