namespace Commerce.EntityLayer.Models
{
    public class ProductFavourites
    {
        public int ProductFavouritesID { get; set; }
        public int FavouritesID { get; set; }
        public Favourites Favourites { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }

}
