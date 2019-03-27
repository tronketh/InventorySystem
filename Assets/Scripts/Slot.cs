using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    bool _isEmpty=true;
    [SerializeField]
    int _id;
    string _itemTag="";
    public Image _iconImageComponent;
    public Text _tagTextComponent,_amountTextComponent;
    public GameObject _backGroundPanel;
    int _itemQuantity=0;
    Sprite _initialSprite;
    GameObject _inventory;
    

    public bool IsEmpty { get => _isEmpty;}
    public GameObject Inventory { get => _inventory; set => _inventory = value; }
    public int Id { get => _id; set => _id = value; }
    public string ItemTag { get => _itemTag; set => _itemTag = value; }
    public int ItemQuantity { get => _itemQuantity; set => _itemQuantity = value; }

    private void Awake()
    {
        _initialSprite = _iconImageComponent.sprite;
        UnSelectSlot();
    }

    public void AddNewItem(string _tag, Sprite _icon, int _amount)
    {
        if (_isEmpty)
        {
            _iconImageComponent.sprite = _icon;
            UpdateTag(_tag);
            UpdateQuantity(_amount);
            _isEmpty = false;
        }
        else if(_tag.Equals(_itemTag))
        {
            AddExistingItem(_amount);
        }
        else
        {
            //ToDo
            Debug.Log("Item Swapped");
            _iconImageComponent.sprite = _icon;
            UpdateTag(_tag);
            UpdateQuantity(_amount);
        }
    }

    public void AddExistingItem(int _amount) {
        if (!_isEmpty)
        {
            _itemQuantity += _amount;
            _amountTextComponent.text = "" + _itemQuantity;
        }
        else
        {
            Debug.Log("Slot " + _id + " Empty");
        }
    }
    public void RemoveExistingItems(int _amount)
    {
        if (!_isEmpty)
        {
            _itemQuantity -= _amount;
            _amountTextComponent.text = "" + _itemQuantity;
            if (_itemQuantity==0)
            {
                RemoveItemFully();
            }
        }
        else
        {
            Debug.Log("Slot "+_id+" Empty");
        }
    }

    /// <summary>
    /// For Selecting item
    /// Returns true when selected and false for empty slot
    /// </summary>
    public void SelectThisSlot() {
        _backGroundPanel.SetActive(true);
        Debug.Log("Slot " + _id + " Selected");
    }

    public void UnSelectSlot() {
        _backGroundPanel.SetActive(false);
    }

    public int RemoveItemFully() {
        int _left = _itemQuantity;
        _isEmpty = true;
        UpdateQuantity(0);
        UpdateTag("");
        _iconImageComponent.sprite = _initialSprite;

        return _left;
    }

    public void UpdateQuantity(int n) {
        _itemQuantity = n;
        _amountTextComponent.text = "" + n;
        if (n == 0)
            _amountTextComponent.text = "";
    }

    public void UpdateTag(string t) {
        _itemTag = t;
        _tagTextComponent.text = t;
    }
}
