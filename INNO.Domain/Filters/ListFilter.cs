namespace INNO.Domain.Filters
{
    public class ListFilter
    {
        public int? TenantId { get; set; }
        public int Page { get; set; }
        public int PageLimit { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? UpdatedAfter { get; set; }

        public int GetPage()
        {
            return (Page < 1) ? 1 : Page;
        }

        public virtual int GetPageLimit()
        {
            if (PageLimit > 50)
            {
                return 50;
            }

            if (PageLimit < 1)
            {
                return 30;
            }

            return PageLimit;
        }
    }
}
