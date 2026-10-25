using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public AudioSource noSlot;

    public bool AddItemToInventory(Sprite itemSprite)
    {
        // Проверяем, есть ли свободные слоты в инвентаре
        bool hasFreeSlot = false;
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.isEmpty)
            {
                hasFreeSlot = true;
                break;
            }
        }

        if (hasFreeSlot)
        {
            // Добавляем предмет в первый свободный слот
            foreach (InventorySlot slot in inventorySlots)
            {
                if (slot.isEmpty)
                {
                    slot.SetItem(itemSprite, slot.slotIndex);
                    return true;
                }
            }
        }
        noSlot.Play();
        return false;
    }
}