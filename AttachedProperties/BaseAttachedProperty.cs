namespace Quan.AttachedProperties
{
    public abstract class BaseAttachedProperty<Parent, Property>
        where Parent : new()
    {
        public static Parent Instansce { get; private set; } = new Parent();
    }
