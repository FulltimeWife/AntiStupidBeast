using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace Anti_Stupid_Beast;

public class Anti_Stupid_BeastSettings : ISettings
{
    //Mandatory setting to allow enabling/disabling your plugin
    public ToggleNode Enable { get; set; } = new ToggleNode(false);

    //TODO: 

    // x,y settings
    // Custom mod filter
    // Custom colour per mod
    // Warn on no imprint
    // Custom colour/x,y for imprint
}

[Submenu(CollapsedByDefault = true)]
public class CustomModFilter {
    public TextNode CustomMod {get; set; } = new ("");
}
