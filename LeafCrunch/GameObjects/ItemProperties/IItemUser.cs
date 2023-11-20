namespace LeafCrunch.GameObjects.ItemProperties
{
    //a thing that can use items
    //mostly just the player
    public interface IItemUser
    {
        void ApplyItem(object args);
    }
}
