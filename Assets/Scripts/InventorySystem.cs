using System;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public ItemBag Bag;
    public GameObject VisualBag;
    public float ThrowForce = 4f;

    public int LockPicks = 3;

    public List<string> Keys = new List<string>();

    private void Start()
    {
        if (Bag)
        {
            VisualBag.SetActive(true);
            Bag.gameObject.SetActive(false);
        }
        else
        {
            VisualBag.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowBag();
        }
    }

    internal void PickUpBag(ItemBag itemBag)
    {
        if (Bag == null)
        {
            Bag = itemBag;
            Bag.gameObject.SetActive(false);
            Bag.transform.parent = transform;
            VisualBag.SetActive(true);
        }
    }

    internal bool HasKey(string v)
    {
        return Keys.Contains(v);
    }

    void ThrowBag()
    {
        if (Bag == null)
            return;

        Bag.transform.position = Camera.main.transform.position + Camera.main.transform.forward;

        VisualBag.SetActive(false);
        Bag.gameObject.SetActive(true);
        Bag.transform.parent = null;
        
        Bag.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * ThrowForce;
        Bag = null;
    }

    
    public void AddLockPicks(int i)
    {
        LockPicks += i;
    }
}