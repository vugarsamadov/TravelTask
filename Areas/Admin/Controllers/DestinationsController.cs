using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelTask.Areas.Admin.Models.Destinations;
using TravelTask.Entities;
using TravelTask.Extensions;
using TravelTask.Persistance;

namespace TravelTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DestinationsController : Controller
    {
        private ApplicationDbContext _dbContext { get; set; }
        private IWebHostEnvironment _env{ get; set; }

        public DestinationsController(ApplicationDbContext context,IWebHostEnvironment env)
        {
            _dbContext = context;
            _env = env;
        }


        public async Task<IActionResult> Index()
        {
            var model = new DestinationsIndexViewModel();
            
            model.Destinations = await _dbContext.Destinations
                .Select(d=>new DestinationsItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Rating  = d.Rating,
                    Price = d.Price,
                    CoverImageUrl = d.CoverImageUrl,
                    IsDeleted = d.IsDeleted
                }).ToListAsync();

            return View(model);
        }


        public async Task<IActionResult> Update(int id)
        {
            var destinaiton = await _dbContext.Destinations.FirstOrDefaultAsync(d => d.Id == id);
            if (destinaiton == null) return NotFound();//TODO notfound view

            var model = new DestinationsUpdateViewModel();

            model.Name = destinaiton.Name;
            model.Price = destinaiton.Price;
            model.CoverImageUrl = destinaiton.CoverImageUrl;
            model.Rating = destinaiton.Rating;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(DestinationsUpdateViewModel model,int id)
        {
           if(!ModelState.IsValid)
            {
                return View(model);
            }

            var destinaiton = await _dbContext.Destinations.FirstOrDefaultAsync(d => d.Id == id);
            if (destinaiton == null) return NotFound();//TODO notfound view

            destinaiton.Name = model.Name;
            destinaiton.Rating = model.Rating;
            destinaiton.Price = model.Price;
            //image save
            if(model.CoverImage != null)
            {
                destinaiton.CoverImageUrl = await model.CoverImage.SaveImageAndProvidePathAsync(FileConstants.DestinationImagesFolderPath,_env);
            }
            
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            var model = new DestinaitonsCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DestinaitonsCreateViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            // save image
            var fileName = await model.CoverImage.SaveImageAndProvidePathAsync(FileConstants.DestinationImagesFolderPath, _env);

            var destination = new Destination()
            {
                Name = model.Name,
                Rating = model.Rating,
                Price = model.Price,
                CoverImageUrl = fileName,
            };

            await _dbContext.Destinations.AddAsync(destination);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var destinaiton = _dbContext.Destinations.FirstOrDefault(x => x.Id == id);
            if (destinaiton == null) return NotFound();
            destinaiton.Delete();
            //_dbContext.Destinations.Remove(destinaiton);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> RevokeDelete(int id)
        {
            var destinaiton = _dbContext.Destinations.FirstOrDefault(x => x.Id == id);
            if (destinaiton == null) return NotFound();
            destinaiton.RevokeDelete();
            //_dbContext.Destinations.Remove(destinaiton);
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

    }
}
