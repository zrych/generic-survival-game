using UnityEngine;

public class HotbarUIManager : MonoBehaviour
{
    [SerializeField] private ItemSlotManager[] slots;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) slots[0].SelectSlot();
        else if (Input.GetKeyDown(KeyCode.Alpha2)) slots[1].SelectSlot();
        else if (Input.GetKeyDown(KeyCode.Alpha3)) slots[2].SelectSlot();
    }
}
