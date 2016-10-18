namespace AmazonSearch.Models
{
    public class SearchedItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DetailPageURL { get; set; }
        public string SmallImage { get; set; }
        public double NewPrice { get; set; }
        public double UsedPrice { get; set; }
        public string CustomerReviewURL { get; set; }
        public string CurrencyCode { get; set; }
    }
}