using LeafCrunch.GameObjects.Items.ItemOperations;

namespace LeafCrunch.GameObjects.ItemProperties
{
    //something that can inflict damage
    public interface IHazard
    {
        Result InflictDamage(GenericGameObject genericGameObject, object paramList);
    }
}
