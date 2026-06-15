using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityDropManager : MonoBehaviour
{
	[SerializeField] private GameObject itemDropPrefab;
	[SerializeField] private ItemListDataSO dropData;

	[Header("Drop restrictions")]
	[SerializeField] private int maxRarityAmount = 1200;
	[SerializeField] private int maxItemsToDrop = 3;
	private bool hasDroppedItems = false;


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
			DropItems();
	}
	public virtual void DropItems()
	{
		if (dropData == null)
			return;
		if (hasDroppedItems) return; // Если уже дропали - выходим
		hasDroppedItems = true;

		List<ItemDataSO> itemsToDrop = RollDrops();
		int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

		for (int i = 0; i < amountToDrop; i++)
		{
			CreateItemDrop(itemsToDrop[i]);
		}
	}

	//public virtual void DropItems()
	//{
	//	Debug.Log($"DROP from {name}");

	//	List<ItemDataSO> itemsToDrop = RollDrops();

	//	Debug.Log($"Items rolled: {itemsToDrop.Count}");

	//	int amountToDrop = Mathf.Min(itemsToDrop.Count, maxItemsToDrop);

	//	Debug.Log($"Items spawned: {amountToDrop}");

	//	for (int i = 0; i < amountToDrop; i++)
	//	{
	//		Debug.Log($"Spawning {itemsToDrop[i].name}");
	//		CreateItemDrop(itemsToDrop[i]);
	//	}
	//}

	protected void CreateItemDrop(ItemDataSO itemToDrop)
	{
		GameObject newItem = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
		newItem.GetComponent<ObjectItemPickup>().SetupItem(itemToDrop);
	}

	public List<ItemDataSO> RollDrops()
	{
		List<ItemDataSO> possibleDrops = new List<ItemDataSO>();
		List<ItemDataSO> finalDrops = new List<ItemDataSO>();
		float maxRarityAmount = this.maxRarityAmount;

		//STEP 1: Roll each item based on rarity and max drop chance
		foreach(var item in dropData.itemList)
		{
			float dropChance = item.GetDropChance();

			if(Random.Range(0, 100) <= dropChance)
				possibleDrops.Add(item);
		}

		//STEP 2: Sort by rarity (highest to lowest)
		possibleDrops = possibleDrops.OrderByDescending(item => item.itemRarity).ToList();

		//STEP 3: Add items to final drop list until rarity limit on entity is reached
		foreach(var item in possibleDrops)
		{
			if(maxRarityAmount >= item.itemRarity)
			{
				finalDrops.Add(item);
				maxRarityAmount -= item.itemRarity;
			}
		}
		return finalDrops;
	}
}
