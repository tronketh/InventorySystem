using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenInventory : MonoBehaviour
{
    //This is list of icon sprite to tag, can be instantiated anywhere else too
    Dictionary<string, Sprite> _objectIconList;

    public GameObject[] _slots;
    public int _lastSelectedID = -1;

    public GameObject ball1, ball2;

    public Sprite ballIcon;

    private void Awake()
    {
        _objectIconList = new Dictionary<string, Sprite>();

        Debug.Log("On Screen Inventory Initialized");
    }

    // Start is called before the first frame update
    void Start()
    {
        //Load here
        //Saved file must contain item tag and corresponding slot number
        /*
        while (eof)
        {
            string tag="";
            int slotnumber=0;
            int quantity = 0;
            //ReadFile
            _slots[slotnumber].GetComponent<Slot>().AddNewItem(tag,_objectIconList[tag],quantity);
        }
        */
        //lastSelectedID=  //Load from some savefile

        #region InitialSlotSelected
        if (_lastSelectedID >= 0 && _lastSelectedID <= 6) {
            _slots[_lastSelectedID].GetComponent<Slot>().SelectThisSlot();
        }
        if (_lastSelectedID == -1)
        {
            SelectNextSlot();
        }
        #endregion

        #region IconLoading
        _objectIconList.Add("Ball", ballIcon);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region ScrollInput
        float _scrollOuput = Input.GetAxis("Mouse ScrollWheel");
        if (_scrollOuput < 0) {
            SelectNextSlot();
        }
        else if (_scrollOuput > 0)
        {
            SelectPreviousSlot();
        }
        #endregion

        #region DropItem
        if (Input.GetKeyDown(KeyCode.Q)) //dropping item
        {
            if (!_slots[_lastSelectedID].GetComponent<Slot>().IsEmpty)
            {
                if (Input.GetKey(KeyCode.LeftShift)) //drop all
                {
                    string _tag = _slots[_lastSelectedID].GetComponent<Slot>().ItemTag;
                    int _quantityDropped = _slots[_lastSelectedID].GetComponent<Slot>().RemoveItemFully();
                    Debug.Log(_quantityDropped + " " + _tag + " in slot " + _lastSelectedID + " has been dropped");
                    //Can be returned for instantiating in real world;
                }
                else
                {
                    string _tag = _slots[_lastSelectedID].GetComponent<Slot>().ItemTag;
                    _slots[_lastSelectedID].GetComponent<Slot>().RemoveExistingItems(1);
                    Debug.Log(1 + " " + _tag + " in slot " + _lastSelectedID + " has been dropped");
                }
            }
        }
        #endregion

        #region PickupItem
        if (Input.GetKeyDown(KeyCode.E)) {
            int pickupQuantity = 1; // change this accordingly
            string tag = "";
            //load gameobject pointing to here in pickupObject
            GameObject pickupObject = ball1;
            if (ball1 == null)
                pickupObject = ball2;

            if (pickupObject != null)
            {
                tag = pickupObject.tag;
                Destroy(pickupObject);
                int i = 0;
                for (; i < 7; i++)
                {
                    if (!_slots[i].GetComponent<Slot>().IsEmpty)
                    {
                        if (_slots[i].GetComponent<Slot>().ItemTag.Equals(tag))
                        {
                            break;
                        }
                    }
                }
                if (i < 7)
                {
                    _slots[i].GetComponent<Slot>().AddExistingItem(pickupQuantity);
                    Debug.Log(pickupQuantity + " " + tag + "(already present) added to Slot " + i);
                }
                else
                {
                    for (i = 0; i < 7; i++)
                    {
                        if (_slots[i].GetComponent<Slot>().IsEmpty)
                        {
                            break;
                        }
                    }
                    if (i < 7)
                    {
                        _slots[i].GetComponent<Slot>().AddNewItem(tag, _objectIconList[tag], pickupQuantity);
                        Debug.Log(pickupQuantity + " " + tag + "(New item) added to Slot " + i);

                    }
                    else
                    {
                        //Item Swapped
                        string _itemtag = _slots[_lastSelectedID].GetComponent<Slot>().ItemTag;
                        int _quantityDropped = _slots[_lastSelectedID].GetComponent<Slot>().RemoveItemFully();
                        Debug.Log(_quantityDropped + " " + _itemtag + " in slot " + _lastSelectedID + " has been dropped");
                        //Can be returned for instantiating in real world;

                        _slots[_lastSelectedID].GetComponent<Slot>().AddNewItem(tag, _objectIconList[tag], pickupQuantity);
                        Debug.Log(pickupQuantity + " " + tag + "(Item swap) added to Slot " + _lastSelectedID);
                    }
                }
            }

        }
        #endregion
    }

    #region SlotSelection
    void SelectNextSlot() {
        if (_lastSelectedID != -1)
            _slots[_lastSelectedID].GetComponent<Slot>().UnSelectSlot();

        _lastSelectedID++;
        if (_lastSelectedID > 6) {
            _lastSelectedID %= 7;
        }
        _slots[_lastSelectedID].GetComponent<Slot>().SelectThisSlot();
        if(!_slots[_lastSelectedID].GetComponent<Slot>().IsEmpty)
            EquipItem(_slots[_lastSelectedID].GetComponent<Slot>().ItemTag);
    }
    void SelectPreviousSlot()
    {
        if (_lastSelectedID != -1)
            _slots[_lastSelectedID].GetComponent<Slot>().UnSelectSlot();
        else
            _lastSelectedID = 0;

        if (_lastSelectedID == 0)
            _lastSelectedID += 7;
        _lastSelectedID--;
        _slots[_lastSelectedID].GetComponent<Slot>().SelectThisSlot();
        if (!_slots[_lastSelectedID].GetComponent<Slot>().IsEmpty)
            EquipItem(_slots[_lastSelectedID].GetComponent<Slot>().ItemTag);
    }
    /// <summary>
    /// Equips Item
    /// </summary>
    /// <param name="tag"></param>
    void EquipItem(string tag) {

    }
    #endregion
}
