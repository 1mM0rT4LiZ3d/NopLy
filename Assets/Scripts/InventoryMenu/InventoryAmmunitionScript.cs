using UnityEngine;

public class InventoryAmmunitionScript : Inventory
{
    private PlayerArmorScript _playerArmorScript;
    private PlayerWeaponScript _playerMainWeaponScript;
    private PlayerWeaponScript _playerSubWeaponScript; //for future

    private void Start()
    {
        _playerArmorScript = FindObjectOfType<PlayerArmorScript>();
        _playerMainWeaponScript = GameObject.FindGameObjectWithTag("MainWeapon").GetComponent<PlayerWeaponScript>();
        _playerSubWeaponScript = GameObject.FindGameObjectWithTag("SubWeapon").GetComponent<PlayerWeaponScript>();
    }

    public override void AddItem(Slot slot)
    {
        if (slot.Info.Type == ItemType.Weapon)
        {
            WeaponInfo weaponInfo = slot.Info as WeaponInfo;

            if (weaponInfo.WeaponPlace == WeaponPlace.Main)
                _playerMainWeaponScript.SetWeapon(weaponInfo);
        }
        else if (slot.Info.Type == ItemType.Armor)
        {
            ArmorInfo armorInfo = slot.Info as ArmorInfo;

            _playerArmorScript.AddArmor(armorInfo);
        }

        UpdateMenu(null);
    }

    public override void DeleteItem(int id)
    {
        if (id == 0)
        {
            _inventoryPlayerScript.AddItem(new Slot(_playerMainWeaponScript.WeaponInfo, 1));
            _playerMainWeaponScript.SetWeapon(null);
        }

        UpdateMenu(null);
    }

    public override void UpdateMenu(Slot[] slots, bool WithText = false)
    {
        if (_playerMainWeaponScript.WeaponInfo != null)
        {
            _images[0].gameObject.SetActive(true);
            _images[0].sprite = _playerMainWeaponScript.WeaponInfo.Sprite;
        }
        else
            _images[0].gameObject.SetActive(false);

        for (int i = 0; i < 2; i++)
        {
            if (_playerArmorScript.Armors[i] != null)
            {
                _images[i + 2].gameObject.SetActive(true);
                _images[i + 2].sprite = _playerArmorScript.Armors[i].Sprite;
            }
            else
                _images[i + 2].gameObject.SetActive(false);
        }
    }

    public override bool CanAddItem(Slot slot) => slot.Info.Type == ItemType.Weapon || slot.Info.Type == ItemType.Armor;

    public void SetPlayerAmmunitionScript(PlayerArmorScript playerArmorScript, out InventoryAmmunitionScript inventoryAmmunitionScript)
    {
        _playerArmorScript = playerArmorScript;
        inventoryAmmunitionScript = this;
    }
}
