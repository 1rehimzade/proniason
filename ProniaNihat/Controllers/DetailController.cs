using ProniaNihat.DAL;
using ProniaNihat.Models;
using ProniaNihat.ModelsVM;
using ProniaNihat.Utilities.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AB460Proniya.Controllers
{
    public class DetailController : Controller
    {
      
        private readonly AppDbContext _context;

        public DetailController(AppDbContext context)
        {
            _context = context;
        }

		public async Task<IActionResult> Detail(int id)
		{
            if (id <= 0) throw new WrongRequestException("Your request is wrong");

            Product product = await _context.Products

                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductTags)
                .ThenInclude(p => p.Tag)
                .Include(p => p.ProductSizes)
                .ThenInclude(p => p.Size)
                .Include(p => p.ProductColors)
                .ThenInclude(p => p.Color)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null) throw new NotFoundException("There is no such product");


            List <Product> RelatedProducts = await _context.Products
                .Include(pi => pi.ProductImages
                .Where(pi => pi.IsPrimary != null))
                .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id)
                .ToListAsync();

            DetailVM vm = new DetailVM()
            {
                Product = product,

                RelatedProducts = RelatedProducts
            };
           



            return View(vm);

        }
    }
}
