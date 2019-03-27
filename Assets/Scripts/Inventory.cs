using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    //This is list of icon sprite to tag, can be instantiated anywhere else too
    Dictionary<string, Sprite> _objectIconList;

    //In game object
    Dictionary<string, int> _itemInventoryList; //Inventory items and quantity
    Dictionary<string, GameObject> _itemSlots; //Inventory items Slots

    public Sprite _ballIcon;

    [SerializeField]
    private string _selectedItemTag;
    [SerializeField]
    private GameObject _selectedItemSlot;
    [SerializeField]
    private bool _somethingSelected;
    
    
    List<GameObject> _slots;
    //Change number of slots here or on Unity
    public int _numberOfSlots=16;
    public int _nextEmptySlotIndex = 0;

    [SerializeField]
    public GameObject _prefabSlot,_contentPane;

    public string SelectedItemTag { get => _selectedItemTag; set => _selectedItemTag = value; }
    public GameObject SelectedItemSlot { get => _selectedItemSlot; set => _selectedItemSlot = value; }
    public bool SomethingSelected { get => _somethingSelected; set => _somethingSelected = value; }

    private void Awake()
    {
        //load icons for each item here
        _objectIconList = new Dictionary<string, Sprite>();
        _objectIconList.Add("Ball", _ballIcon);
        //Load inventory items here
        _itemInventoryList = new Dictionary<string, int>();

        _itemSlots = new Dictionary<string, GameObject>();

        _slots = new List<GameObject>();
           
        for(int i=0;i<_numberOfSlots;i++) {
            AddSlot();
        }
        foreach(KeyValuePair<string,int> element in _itemInventoryList)
        {
            _itemSlots.Add(element.Key,_slots[_nextEmptySlotIndex]);
            /*
            _itemSlots.Add(element.Key,Instantiate(_prefabSlot));
            _itemSlots[element.Key].transform.SetParent(_contentPane.transform);
            */
            _itemSlots[element.Key].GetComponent<Toggle>().group = _contentPane.GetComponent<ToggleGroup>();
            _itemSlots[element.Key].GetComponent<Slot>().AddNewItem(element.Key, _objectIconList[element.Key], element.Value);
            _nextEmptySlotIndex++;
        }
        Debug.Log("Inventory Initialized");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) //dropping item
        {
            if (SomethingSelected)
            {
                if (Input.GetKey(KeyCode.LeftShift)) //drop all
                {
                    int _quantityDropped = ReturnLeftOver(_selectedItemTag);
                    //Can be returned for instantiating in real world;
                }
                else
                {
                    //Code to instantiate in real world
                    RemoveObjects(_selectedItemTag, 1);
                }
            }
            else
            {
                //Code for sound output if nothing selected or Empty Slot is selected
            }
        }
        //to be improved
        else if (Input.GetKey(KeyCode.E)) { //eating, drinking or applying item or building
            if (SomethingSelected)
            {
                //code to add item effect to player
                bool _removable=RemoveObjects(_selectedItemTag, 1);
            }
        }
    }

    public bool AddObject(string _tag,int _quantity) {

        if (_itemInventoryList.ContainsKey(_tag))
        {
            _itemInventoryList[_tag] += _quantity;
            _itemSlots[_tag].GetComponent<Slot>().AddExistingItem(_quantity);
            return true;
        }
        else
        {
            if (_itemInventoryList.Count < _numberOfSlots)
            {
                _itemInventoryList.Add(_tag, _quantity);
                _itemSlots.Add(_tag, _slots[_nextEmptySlotIndex]);
                _itemSlots[_tag].GetComponent<Slot>().AddNewItem(_tag, _objectIconList[_tag], _quantity);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public bool RemoveObjects(string _tag, int _quantity) {
        if (_itemInventoryList[_tag] > _quantity) {
            _itemInventoryList[_tag] -= _quantity;
            _itemSlots[_tag].GetComponent<Slot>().RemoveExistingItems(_quantity);
            return true;
        }
        else if (_itemInventoryList[_tag] == _quantity) {
            _itemSlots[_tag].GetComponent<Slot>().RemoveItemFully();
            _itemInventoryList.Remove(_tag);
            RemoveSlot(_tag);
            return true;
        }
        else
        {
            Debug.Log("Not Enough Item");
            return false;
        }
    }

    public int ReturnLeftOver(string _tag) {
        int left = _itemSlots[_tag].GetComponent<Slot>().RemoveItemFully();
        _itemInventoryList.Remove(_tag);
        RemoveSlot(_tag);
        return left;
    }

    public void RemoveSlot(string _tag) {
        _slots.Remove(_itemSlots[_tag]);
        Destroy(_itemSlots[_tag]);
        _itemSlots.Remove(_tag);
        _nextEmptySlotIndex--;
        SomethingSelected = false;
        AddSlot();
    }

    void AddSlot() {
        GameObject _slot = Instantiate(_prefabSlot);
        _slot.transform.SetParent(_contentPane.transform);
        _slot.GetComponent<Toggle>().group = _contentPane.GetComponent<ToggleGroup>();
        _slots.Add(_slot);
        _slot.transform.localScale = Vector3.one;
        _slot.GetComponent<Slot>().Inventory = gameObject;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }
    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}
