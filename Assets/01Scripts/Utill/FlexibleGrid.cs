using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleGrid : MonoBehaviour
{
    private float originWidth, originHeight;
    public RectTransform parent;
    public GridLayoutGroup grid;

    private void Awake()
    {
        parent = gameObject.GetComponent<RectTransform>();
        grid = gameObject.GetComponent<GridLayoutGroup>();

        originWidth = parent.rect.width;
        originHeight = parent.rect.height;
    }


    [ContextMenu("FlexibleGrid")]
    public void SetFlexibleGrid(int _childCount = -1)
    {
        int childCount = _childCount;

        if (childCount == -1)
        {
            childCount = gameObject.transform.childCount;
        }

        float cell_width = grid.cellSize.x;
        float cell_height = grid.cellSize.y;

        float space_height = grid.spacing.y;

        int constraintCount = grid.constraintCount;

        float increseSize = (cell_height + space_height) * Mathf.Ceil((float)childCount / constraintCount) + grid.padding.top + grid.padding.bottom;

        parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, increseSize);
    }

    public void SetFilexibleHorizontal()
    {
        float cell_width = grid.cellSize.x;
        float cell_height = grid.cellSize.y;

        float space_height = grid.spacing.y;

        int constraintCount = grid.constraintCount;

        float increseSize = (cell_height + space_height) * Mathf.Ceil((float)gameObject.transform.childCount / constraintCount) + grid.padding.top + grid.padding.bottom;

        parent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, increseSize);
    }

}
