namespace WorkingMVC.Interfaces
{
    public interface IHiddenCategoryService
    {
        //додає до списку прихованих категорій id категорії
        void Hide(int id);
        //перевіряє чи є категорія прихованою
        bool IsHidden(int id);
        //повертає список прихованих категорій
        IEnumerable<int> GetHiddenIds();
    }

}
