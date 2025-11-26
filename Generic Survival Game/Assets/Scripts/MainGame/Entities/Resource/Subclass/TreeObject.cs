using UnityEngine;

public class TreeObject : ResourceNode
{
    protected override void BreakNode()
    {
        //TODO: Drop item

        gameObject.SetActive(false); //Tree disappears
    }
}
