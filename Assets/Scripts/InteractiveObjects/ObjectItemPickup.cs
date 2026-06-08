using UnityEngine;

public class ObjectItemPickup : MonoBehaviour
{
    [SerializeField] private Vector2 dropForce = new Vector2(3, 10);
    [SerializeField] private ItemDataSO itemData;

    [Space]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;

    //private void Awake()
    //{
    //    itemToAdd = new InventoryItem(itemData);
    //}

    private void OnValidate()
    {
        if (itemData == null)
            return;

        sr = GetComponent<SpriteRenderer>();
        SetupVisuals();
    }

    public void SetupItem(ItemDataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        float xDropForce = Random.Range(-dropForce.x, dropForce.x);
        rb.linearVelocity = new Vector2(xDropForce, dropForce.y);
        col.isTrigger = false;
    }

    private void SetupVisuals()
    {
        sr.sprite = itemData.itemIcon;
        gameObject.name = "ObjectItemPickup - " + itemData.itemName;
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") && col.isTrigger == false)
        {
            col.isTrigger = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        //inventory = collision.GetComponent<InventoryBase>();
        InventoryPlayer inventory = collision.GetComponent<InventoryPlayer>();
		InventoryItem itemToAdd = new InventoryItem(itemData);

		if (inventory == null)
            return;

        bool canAddItem = inventory.CanAddItem() || inventory.FindStackable(itemToAdd) != null;

        if(canAddItem)
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
