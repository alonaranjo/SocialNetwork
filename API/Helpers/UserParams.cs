
namespace API.Helpers
{
    public class UserParams: PaginationParams
    {
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