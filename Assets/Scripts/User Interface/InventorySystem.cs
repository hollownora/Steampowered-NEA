using System.Collections.Generic;

public class Inventory
{
	// list used to store items in inventory. has a size limit of 10
	protected List<Item> playerInventory = new List<Item>();

	public void AddItem(Item itemToAdd, int quantity=1)
    {
		// loop for the given quantity and try to add the items
		if (quantity < 9 - playerInventory.Count)
        {
			// add the items
			for (int counter = 0; counter < quantity; counter++)
            {
				playerInventory.Add(itemToAdd);
            }
        }
    }

	public bool IsInventoryFull()
    {
		return playerInventory.Count == 9;
    }

	public Item UseItem(int itemID)
    {
		Item tmpStore = playerInventory[itemID];
		// remove item from list
		playerInventory.RemoveAt(itemID);
		// return item
		return tmpStore;
    }

	public Item GetItem(int ItemIndex)
    {
		return playerInventory[ItemIndex];
    }

	public int GetNoOfItems()
    {
		return playerInventory.Count;
    }
}

public class Item
{
	protected string itemName;
	protected ItemType itemType;
	protected int healNum;

	public Item(string name, ItemType type, int healStat, int? lowerDamageBound=null, int? upperDamageBound=null)
    {
		// an item is defined by name and the type of item. the item type determines who can be targeted
		itemName = name;
		itemType = type;
		healNum = healStat;
    }

	// getter methods

	public int GetHealValue()
    {
		return healNum;
    }

	public ItemType GetItemType()
    {
		return itemType;
    }

	public string GetName()
    {
		return itemName;
    }
}

public enum ItemType
{
	healing,
	damage,
	statChange
}