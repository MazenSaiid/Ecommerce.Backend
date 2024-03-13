namespace Ecom.Backend.Core.Sharing
{
    public class ProductParams
    {
        public int MaxPageSize { get; set; } = 15;
        public int? CategoryId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
        private int _pageSize = 3;

        private string _search;

        public string Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }

        public string Sort { get; set; }
    }
}
