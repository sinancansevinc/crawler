using crawler.Context;
using crawler.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text;
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

		public IActionResult AddDatabase(string q ) {

			using (var driver = new ChromeDriver())
			{
				driver.Navigate().GoToUrl("https://b2b.haskar.com.tr/giris?ReturnUrl=%2f");

				var user = driver.FindElementById("Email");
				var pass = driver.FindElementById("Password");

				var button = driver.FindElementByXPath("//input[@value='GİRİŞ']");

				user.SendKeys("coskunlastikjant@gmail.com");
				pass.SendKeys("111111");

				button.Click();

				var search = driver.FindElementById("small-searchterms");
				search.SendKeys(q);
				var button2 = driver.FindElementByXPath("//input[@value='ARA']");

				button2.Click();

				var result2 = driver.FindElementsByClassName("item-box_list_row");



				foreach (var item in result2)
				{
					Product product = new Product();
					product.StockCode = item.FindElement(By.ClassName("kodu")).Text;
					product.StockName = item.FindElement(By.ClassName("adi")).Text;
					product.Brand = item.FindElement(By.ClassName("marka")).Text;
					product.Price = item.FindElement(By.ClassName("KdvHaricNetFiyat")).Text;

					_db.Products.Add(product);
					_db.SaveChanges();
				}
				
			}

			return RedirectToAction("List");


		}

		public IActionResult List()
		{
			return View(_db.Products.ToList());
		}

	

	}
}
