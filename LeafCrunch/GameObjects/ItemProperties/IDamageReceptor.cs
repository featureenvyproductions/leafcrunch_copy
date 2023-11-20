namespace LeafCrunch.GameObjects.ItemProperties
{
    //something that can take damage
    public interface IDamageReceptor
    {
        //for the player, damage is point decrements
        //but for anything else if we expand this functionality
        //it might effect speed or maybe destroy something altogether?
        void ApplyDamage(object args);
    }
}
