using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelTask.Areas.Admin.Models;
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


        public async Task<IActionResult> PaginatedIndex(int page=1,int per_page=3)
        {
            var model = new DestinationsIndexViewModel();
            


            var pmodel = new PagedEntityModel<List<DestinationsItemViewModel>>();
            pmodel.Page = page;
            pmodel.PageSize = per_page;
            pmodel.PageCount = (int)Math.Ceiling((decimal)await _dbContext.Destinations.CountAsync() / per_page);

            var routevaldic = new RouteValueDictionary();

            routevaldic["page"] = page + 1;
            routevaldic["per_page"] = per_page;

            pmodel.Next = Url.Action(nameof(PaginatedIndex), routevaldic);
            routevaldic["page"] = page - 1;
            pmodel.Prev = Url.Action(nameof(PaginatedIndex), routevaldic);

            pmodel.Data  = await _dbContext.Destinations
                .Skip((page-1)*per_page)
                .Take(per_page)
                .Select(d => new DestinationsItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Rating = d.Rating,
                    Price = d.Price,
                    CoverImageUrl = d.CoverImageUrl,
                    IsDeleted = d.IsDeleted
                }).ToListAsync();


            return PartialView("_PaginatedEntityTable",pmodel);
        }


        public async Task<IActionResult> Index()
        {
            var model = new DestinationsIndexViewModel();

            var page = 1;
            var per_page = 3;

            var pmodel = new PagedEntityModel<List<DestinationsItemViewModel>>();
            var routevaldic = new RouteValueDictionary();

            routevaldic["page"] = page + 1;
            routevaldic["per_page"] = per_page;

            pmodel.Next = Url.Action(nameof(PaginatedIndex), routevaldic);
            routevaldic["page"] = page - 1;
            pmodel.Prev = Url.Action(nameof(PaginatedIndex), routevaldic);

            pmodel.Page = page;
            pmodel.PageSize = per_page;
            pmodel.PageCount =(int) Math.Ceiling((decimal)await _dbContext.Destinations.CountAsync()/per_page);
            pmodel.Data = await _dbContext.Destinations
                .Skip((page - 1) * per_page)
                .Take(per_page)
                .Select(d => new DestinationsItemViewModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Rating = d.Rating,
                    Price = d.Price,
                    CoverImageUrl = d.CoverImageUrl,
                    IsDeleted = d.IsDeleted
                }).ToListAsync();


            return View(pmodel);
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
