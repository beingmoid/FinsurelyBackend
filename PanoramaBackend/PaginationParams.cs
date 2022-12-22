using System;

namespace PanoramaBackend.Api
{
    public class PaginationParams<TKey>
       
    {
        public TKey Id { get; set; }
        public DateTime? from{ get; set; }
        public DateTime? to { get; set; }
        public int? BranchId { get; set; }

        public string? SearchQuery { get; set; }
        public string? MyProperty { get; set; }


        private const int _maxItemPerPage = 50;
        private int itemsPerPage;
        public int Page { get; set; }
        public int ItemsPerPage
        {
            get { return itemsPerPage; }
            set=> itemsPerPage = value > _maxItemPerPage?_maxItemPerPage : value;

        }
    }
}
