using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public GameObject _inventory;

    //For Test
    public GameObject _ball;
    public GameObject _ball2;
    //For Test

    /// <summary>
    ///Will add the object to a inventory list 
    /// </summary>
    public void PickUpObject(GameObject _pickedUpObject) {
        _inventory.GetComponent<Inventory>().AddObject(_pickedUpObject.tag,1);
        Destroy(_pickedUpObject);
    }
    public void DropObject()
    {

    }
    private void Start()
    {
        _inventory.SetActive(true);
        _inventory.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            EnableInventory();
        }

        //For Test
        else if (Input.GetKeyDown(KeyCode.E)) { //add your object to be pickedup
            if(_ball!=null)
                PickUpObject(_ball);
            if (_ball2 != null)
                PickUpObject(_ball2);
        }
        //For Test
    }

    
    void EnableInventory() {
        _inventory.SetActive(!_inventory.activeSelf);
    }
}
