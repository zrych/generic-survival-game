using UnityEngine;

public class Tree : ResourceNode
{
    protected override void BreakNode()
    {
        //TODO: Drop item

        gameObject.SetActive(false); //Tree disappears
    }
}
