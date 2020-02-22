using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite ChestOpened;
    
    private SpriteRenderer mySpriteRenderer;

    private int touched;

    private System.Random prng;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        touched = 0;

    }

    public void SetSeed(int seed)
    {
        prng = new System.Random(seed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            mySpriteRenderer.sprite = ChestOpened;
            if (touched < 1)
            {

              Vector3 aboveChest = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

                List<Item.ItemType> ListOfAllItemTypes = ItemWorld.AllItemTypesList();
                Item.ItemType randomItemTypeFromThatList = ListOfAllItemTypes[prng.Next(ListOfAllItemTypes.Count)];


              ItemWorld thisItem = ItemWorld.SpawnItemWorld(aboveChest, new Item { itemType = randomItemTypeFromThatList, amount = 1 } );
              thisItem.SpawnOutOfChest();
            }


            touched++;
        }
    }
}
