using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WellcomeW : MonoBehaviour
{
    //public Animator Animator;

    private float originWidth, originHeight;
    public RectTransform parent;
    public GridLayoutGroup Maingrid;
    public GridLayoutGroup solugrid;
    private void Start()
    {
        originWidth = parent.rect.width;
        originHeight = parent.rect.height;
        SetDynamicGrid(Maingrid, 4, 2, 2);
        SetDynamicGrid(solugrid, 9, 3, 3);
        //ClickWc();
    }
    public void ClickWc()
    {
        //Animator.SetTrigger("Login");
    }


    // cnt : ÄÃ·³ ÃÑ °¹¼ö, minColsInARow : ÇÑ Row¿¡ ÃÖ¼Ò ÄÃ·³ °¹¼ö, maxRow : ÃÖ´ë Row ¼ö.
    public void SetDynamicGrid(GridLayoutGroup grid, int cnt, int minColsInARow, int maxRow)
    {
        int rows = Mathf.Clamp(Mathf.CeilToInt((float)cnt / minColsInARow), 1, maxRow + 1);
        int cols = Mathf.CeilToInt((float)cnt / rows);
        float spaceW = (grid.padding.left + grid.padding.right) + (grid.spacing.x * (cols - 1));
        float spaceH = (grid.padding.top + grid.padding.bottom) + (grid.spacing.y * (rows - 1));
        float maxWidth = originWidth - spaceW;
        float maxHeight = originHeight - spaceH;
        float width = Mathf.Min(parent.rect.width - (grid.padding.left + grid.
        padding.right) - (grid.spacing.x * (cols - 1)), maxWidth);
        float height = Mathf.Min(parent.rect.height - (grid.padding.top + grid.
        padding.bottom) - (grid.spacing.y * (rows - 1)), maxHeight);
        //Debug.Log("W: " + width + " H: " + height);
        if(width>300)
            grid.cellSize = new Vector2(150, 150);
        else
            grid.cellSize = width<height? new Vector2(width / cols, (width / rows)*0.86f): new Vector2(height / cols, (height / rows)*0.86f);
    }
}
