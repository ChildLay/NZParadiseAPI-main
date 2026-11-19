using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZParadiseAPI.Library;
using NZParadiseAPI.Model.Abstract;
using NZParadiseAPI.Model.Entities;

namespace NZParadiseAPI.Model.Concrete
{
    public class WishlistsRepository
    {
        private readonly ApplicationDbContext _context;
        public WishlistsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool IsPropertyWishlistedByUser(int userID, int propertyID)
        {
            var wishlistInfo = _context.Wishlists
            .Where(x => x.UserID == userID && x.PropertyID == propertyID).FirstOrDefault();

            return wishlistInfo != null;
        }

        public async Task<Wishlist> GetWishlistData(int userID, int propertyID)
        {
            var wishlistInfo = await _context.Wishlists
                .Where(x => x.UserID == userID && x.PropertyID == propertyID)
                .FirstOrDefaultAsync();

            return wishlistInfo;
        }

        public async Task<Boolean> Save(Wishlist wl)
        {

            if (wl == null)
                return false;

            _context.Wishlists.Add(wl);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Boolean> Delete(Wishlist wl)
        {
            if (wl == null)
                return false;

            _context.Wishlists.Remove(wl);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteByPropertyID(int propertyID)
        {
            var wishlists = _context.Wishlists.Where(w => w.PropertyID == propertyID).ToList();
            
            foreach (var w in wishlists)
            {
                _context.Wishlists.Remove(w);
                await _context.SaveChangesAsync();
            }
        }
    }
}
