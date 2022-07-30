using API.Helpers.Pagination;

namespace API.Models.Parameters
{
    public class UserParameters: PaginationParams
    {
        public bool CoachesOnly { get; set; } = false;

        public bool NotInATeam { get; set; } = false;

    }
}