using WorkingMVC.Interfaces;

namespace WorkingMVC.Services
{
    public class HiddenCategoryService : IHiddenCategoryService
    {
        //Приватне поле для збереження прихованих id категорій
        //HashSet - колекція унікальних значень та швидким пошуком
        private readonly HashSet<int> hiddenIds = new();
        public void Hide(int id) => hiddenIds.Add(id);
        public bool IsHidden(int id) => hiddenIds.Contains(id);
        public IEnumerable<int> GetHiddenIds() => hiddenIds;
    }

}
