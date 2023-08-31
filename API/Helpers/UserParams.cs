
namespace API.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        public int pageNumber  { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }  
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }    
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 100;
        public DateOnly MinDob
        {
            get => DateOnly.FromDateTime(DateTime.Today.AddYears(-MaxAge - 1));
        }
        public DateOnly MaxDob
        {
            get => DateOnly.FromDateTime(DateTime.Today.AddYears(-MinAge));
        }
        public string OrderBy { get; set; }
    }
}