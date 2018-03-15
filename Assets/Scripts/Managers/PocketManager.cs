using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PocketManager : Singleton<PocketManager>
{
    [SerializeField]
    private GenericGrid _pocketGrid = null;

    private GridElement _selectedPocket = null;
    public GridElement SelectedPocket
    {
        get { return _selectedPocket; }
        set
        {
            _selectedPocket = value;
            setPocketSelectedAppearance();
        }
    }
    // ====================================================================================================== //
    void Start ()
    {
        Party.Instance.Event_SelectedPartyMemberChanged += onSelectedPartyMemberChanged;
    }
    // ====================================================================================================== //
    void Update()
    {
        checkClickOnPocket();
    }
    // ====================================================================================================== //
    private void checkClickOnPocket()
    {
        if (WindowManager.Instance.IsCurrentWindow(Consts.WindowNames.DUNGEON) && Input.GetMouseButtonDown(0))
        {
            Pocket pocket = Utils.GetObjectUnderCursor<Pocket>("Pocket");
            if (pocket != null)
            {
                Debug.Log("Pocket selected!");
                SelectedPocket = pocket;
            }
        }
    }
    // ====================================================================================================== //
    public void AddItem(Item item, GameObject pocket)
    {
        item.transform.position = pocket.transform.position;
        item.transform.parent = pocket.transform;
        item.State = ItemState.IN_POCKET;
    }
    // ====================================================================================================== //
    private void setPocketSelectedAppearance()
    {
        foreach (GridElement pocket in _pocketGrid.Elements)
        {
            pocket.GetComponent<SpriteRenderer>().color = (pocket == _selectedPocket) ? Color.red : Color.white;
        }
    }
    // ====================================================================================================== //
    // triggered on Party -> Event_SelectedPartyMemberChanged event
    private void onSelectedPartyMemberChanged()
    {
        showSelectedMemberPockets();
    }
    // ====================================================================================================== //
    private void showSelectedMemberPockets()
    {
        // units visibility & color of grid units
        _pocketGrid.UpdateElementsVisibility(Party.Instance.SelectedMember.Stats[Stat.POCKETS]);

        // hide/show items on belt by the selected member
        List<Item> selectedMemberBeltItems = Party.Instance.SelectedMember.ItemsInPockets.Values.ToList();
        foreach (Transform beltSlotTransform in this.transform)
        {
            foreach (Transform itemTransform in beltSlotTransform)
            {
                Item item = itemTransform.GetComponent<Item>();
                itemTransform.gameObject.SetActive(selectedMemberBeltItems.Contains(item));
            }
        }

        // init to non-selected pockets
        SelectedPocket = null;
    }
    // ====================================================================================================== //
}
