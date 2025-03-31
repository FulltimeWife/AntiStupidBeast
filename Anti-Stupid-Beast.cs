using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Components;

namespace Anti_Stupid_Beast;

public class Anti_Stupid_Beast : BaseSettingsPlugin<Anti_Stupid_BeastSettings>
{
    private readonly SharpDX.Color defaultColor = SharpDX.Color.White;
    private readonly SharpDX.Color intelligenceColor = SharpDX.Color.Aqua;
    private readonly SharpDX.Color graceColor = SharpDX.Color.Green;
    private readonly SharpDX.Color enduranceColor = SharpDX.Color.Red;
    private readonly SharpDX.Color finishedColor = SharpDX.Color.Gold;
    private int textPositionY = 350; // TODO Set up a settings file that I can adjust it in game
    public override bool Initialise()
    {
        return true;
    }


    public override void Render()
    {
        var bloodAltarWindow = GetBloodAltarWindow();
        if (!bloodAltarWindow.IsVisible) {
            GameController.EntityListWrapper.RefreshState();
            return;
        }

        var bloodAltarItem = GetBloodAltarItem(bloodAltarWindow);

        if (bloodAltarItem == null) return;

        var modsComponent = bloodAltarItem.Entity.GetComponent<Mods>();
        if (modsComponent == null) {
            DebugWindow.LogMsg("Blank item");
            return;
        }
        DisplayCurrentMods(modsComponent);
        WarnOnNoImprint();
    }

    private Element GetBloodAltarWindow() {
        return GameController.IngameState.IngameUi.GetChildAtIndex(73);
    }

    private Element GetBloodAltarItem(Element bloodAltarWindow) {
        var bloodAltarBottom = bloodAltarWindow?.GetChildAtIndex(6);
        return bloodAltarBottom?.GetChildAtIndex(1)?.GetChildAtIndex(1);

    }

    private void DisplayCurrentMods(Mods modsComponent) {
        if (modsComponent.HumanImpStats == null) return;

        foreach (var mod in modsComponent.HumanImpStats) {
            var color = DetermineModColor(mod);

            Graphics.DrawText(
                mod,
                new System.Numerics.Vector2(400, textPositionY),
                color
            );
            textPositionY += 20;
        }
        // TO DO: Clean this up & implement better to return Y back to original state
        textPositionY = 350;
    }

    private SharpDX.Color DetermineModColor(string modText) {
        bool containsGrace = modText.Contains("Grace");
        bool containsIntelligence = modText.Contains("Increased Intelligence");
        bool containsEndurance = modText.Contains("Endurance");

        if (containsGrace && containsIntelligence) return finishedColor;
        if (containsGrace) return graceColor;
        if (containsEndurance) return enduranceColor;
        if (containsIntelligence) return intelligenceColor;

        return defaultColor;
    }

    public List<CustomItemData> GetInventoryItems() {
        var inventoryItems = new List<CustomItemData>();
        if (!IsInventoryVisible()) return inventoryItems;

        var inventory = GameController?.Game?.IngameState?.Data?.ServerData?.PlayerInventories[0]?.Inventory;
        var items = inventory?.InventorySlotItems;

        if (items == null) return inventoryItems;

        foreach (var item in items) {
            if(item.Item == null || item.Address == 0) continue;

            inventoryItems.Add(new CustomItemData(item.Item, GameController, item.GetClientRect()));
        }

        return inventoryItems;
    }

    private bool IsInventoryVisible()
    {
        return GameController.IngameState.IngameUi.InventoryPanel.IsVisible;
    }

    private void WarnOnNoImprint() {
        bool foundImprint = false;
        var items = GetInventoryItems();

        foreach (var customItem in items) {
            var itemEntity = customItem.Item;

            if (itemEntity?.IsValid != true) continue;

            var baseComp = itemEntity.GetComponent<Base>();
            if (baseComp?.Name?.Contains("Imprint") == true) {
                foundImprint = true;
                break;
            }
        }

        /* TO DO: 
        Figure out how to change font size so that the warning is larger,
        Find a better coordinate to rest the warning on
        */

        if (!foundImprint) {
            Graphics.DrawText(
                "No Imprint Detected",
                new System.Numerics.Vector2(400, textPositionY - 40),
                SharpDX.Color.Red
            );
        }
    }
}