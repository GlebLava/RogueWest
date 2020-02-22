using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{

    private Item item;
    private SpriteRenderer mySpriteRenderer;
    private Collider2D myCollider2D;


    [System.NonSerialized]
    private bool hovering;

    public static ItemWorld SpawnItemWorld(Vector2 position, Item item)
    {
        GameObject thisObject = Instantiate(ItemAssets.Instance.ItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = thisObject.GetComponent<ItemWorld>();

        itemWorld.SetItem(item);

        return itemWorld;
    }

    public static ItemWorld DropItem(Item item, Vector2 dropPosition)
    {
        ItemWorld thisItem = SpawnItemWorld(dropPosition, item);
        return thisItem;
    }

    public static List<Item.ItemType> AllItemTypesList() {
        return new List<Item.ItemType>() {Item.ItemType.Coin, Item.ItemType.HealthPotion, Item.ItemType.ManaPotion, Item.ItemType.MedKit, Item.ItemType.Sword };

    }

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myCollider2D = GetComponent<Collider2D>();
    }
    private void FixedUpdate()
    {
        if (hovering) transform.position = new Vector2(transform.position.x, transform.position.y + LetThisItemWorldHover(0.005f, 2f));
    }

   
    public void SetItem(Item item)
    {
        this.item = item;
        mySpriteRenderer.sprite = item.GetSprite();
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void SpawnOutOfChest()
    {
        hovering = true;
        StartCoroutine(DeactivateColliderFor(0.5f));
        
    }



    private float LetThisItemWorldHover(float amplitude, float speed)
    {
        return amplitude * Mathf.Sin(speed * Time.time);
    }
    IEnumerator DeactivateColliderFor(float seconds)
    {
        myCollider2D.enabled = false;
        yield return new WaitForSeconds(seconds);
        myCollider2D.enabled = true;
    }
 

}

