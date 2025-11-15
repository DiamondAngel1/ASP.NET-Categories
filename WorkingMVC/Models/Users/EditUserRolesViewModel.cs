namespace WorkingMVC.Models.Users
{
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> AllRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
    }

}
