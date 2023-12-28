namespace HotelsApplication.Models.Pagination
{
    public class QueryParameters
    {
        private int _pageSize;
        public int PageNumber { get; set; }
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
    }
}
