using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    bool _isEmpty=true;
    string _itemTag="";
    public Image _iconImageComponent;
    public Text _tagTextComponent,_amountTextComponent;
    int _itemQuantity=0;
    Sprite _initialSprite;
    GameObject _inventory;
    

    public bool IsEmpty { get => _isEmpty; set => _isEmpty = value; }
    public int ItemQuantity { get => _itemQuantity; set => _itemQuantity = value; }
    public GameObject Inventory { get => _inventory; set => _inventory = value; }

    private void Awake()
    {
        _initialSprite = _iconImageComponent.sprite;
    }

    public void AddNewItem(string _tag, Sprite _icon, int _amount)
    {
        _iconImageComponent.sprite = _icon;
        _tagTextComponent.text = _tag;
        _amountTextComponent.text =""+_amount;

        _itemTag = _tag;
        _itemQuantity = _amount;
        _isEmpty = false;
    }
    public void AddExistingItem(int _amount) {
        _itemQuantity += _amount;
        _amountTextComponent.text = "" + _itemQuantity;
    }

    /// <summary>
    /// For Selecting item
    /// </summary>
    public void PickItem() {
        if (!_isEmpty)
        {
            _inventory.GetComponent<Inventory>().SelectedItemTag = _itemTag;
            _inventory.GetComponent<Inventory>().SelectedItemSlot = gameObject;
            _inventory.GetComponent<Inventory>().SomethingSelected = true;
        }
    }

    public void RemoveObjects(int _amount) {
        _itemQuantity -= _amount;
        _amountTextComponent.text = "" + _itemQuantity;
    }


    public int RemoveItemFully() {
        int _left = _itemQuantity;
        _isEmpty = true;
        _itemTag = "";
        _itemQuantity = 0;

        _iconImageComponent.sprite = _initialSprite;
        _tagTextComponent.text = "";
        _amountTextComponent.text = "";
        return _left;
    }
}
