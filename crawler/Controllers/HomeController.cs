using crawler.Context;
using crawler.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace crawler.Controllers
{
	public class HomeController : Controller
	{


		public HomeController()
		{

		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult List()
		{
			var db = new DatabaseContext();            
		    return View(db.SportCars.ToList());
		}
		
		public IActionResult Search(string q)
		{

			StartCrowlerAsync(q);
			return RedirectToAction("List");

		}
		public static async Task StartCrowlerAsync(string q)
		{


			q = q.ToLower();
			var url = "https://www.arabam.com/ikinci-el?searchText=" + q;
			var httpClient = new HttpClient();
			var html = await httpClient.GetStringAsync(url);

			var htmlDocument = new HtmlDocument();

			htmlDocument.LoadHtml(html);
			var tr = htmlDocument.DocumentNode.Descendants("tr").Where(node => node.GetAttributeValue("class", "").Contains("listing-list-item")).ToList();

			foreach (var item in tr)
			{SportCar car = new SportCar();
				car.Title = item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("listing-modelname pr")).FirstOrDefault().InnerText;

				car.Year =item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("listing-text pl8 pr8 tac pr")).FirstOrDefault().InnerText;

				var priceTd = item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("pl8 pr8 tac pr")).FirstOrDefault().InnerText;

				//priceTd = priceTd.Substring(0, priceTd.Length - 4);

				car.Price = priceTd;

				var locationTd = item.Descendants("td").Where(p => p.GetAttributeValue("class", "").Equals("listing-text pl8 pr8 tac pr")).LastOrDefault();
				car.Location = locationTd.Descendants("div").Where(p => p.GetAttributeValue("class", "").Equals(" fade-out-content-wrapper")).FirstOrDefault().InnerText;
				using (var db=new DatabaseContext())
				{
					db.SportCars.Add(car);
					await db.SaveChangesAsync();
				}
				
				//carPrice = carPrice.Replace("  ", "");



				//await _db.SportCars.AddAsync(car);
				//await _db.SaveChangesAsync();
				

		


				//carPrice = carPrice.Replace("\n", "");
			}
		}
		//public void AddDatabase(SportCar car)
		//{
		//	try
		//	{
		//		_db.SportCars.AddAsync(car);
		//		_db.SaveChangesAsync();
		//	}
		//	catch (Exception)
		//	{

		//		Console.WriteLine("Hata");
		//	}


		//}


	}
}
