using ExileCore;
using ExileCore.PoEMemory.MemoryObjects;

public class CustomItemData
{
    public Entity Item { get; }
    public GameController GameController { get; }
    public SharpDX.RectangleF ClientRect { get; }

    public CustomItemData(Entity item, GameController gc, SharpDX.RectangleF rect)
    {
        Item = item;
        GameController = gc;
        ClientRect = rect;
    }
}