﻿using System.ComponentModel.DataAnnotations;

namespace Commerce.EntityLayer.Models
{
    public class Favourites
    {
        public int FavouritesID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
        public List<ProductFavourites> ProductFavourites { get; set; } = [];
    }
}
