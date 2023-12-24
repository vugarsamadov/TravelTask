using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TravelTask.Areas.Admin.Controllers;
using TravelTask.Models.Destinations;
using TravelTask.Models;
using TravelTask.Persistance;

namespace TravelTask.Controllers
{
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _dbContext;

		public HomeController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;	
		}

		public async Task<IActionResult> Index()
		{
			var destinations = await _dbContext.Destinations.Where(d => !d.IsDeleted).ToListAsync();

			var model = new DestinationsIndexViewModel();

			model.Destinations = destinations
				.Select(d=>new DestinationItemViewModel
				{
					Name = d.Name,
					Rating =	d.Rating,
					Price = d.Price,
					CoverImageUrl = d.CoverImageUrl
				}).ToList();

			return View(model);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
