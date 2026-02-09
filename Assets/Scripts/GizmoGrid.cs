using UnityEngine;

public class GizmoGrid : MonoBehaviour
{

    bool displayGrid = true;

    bool isIsometric = false;

    float xCount;
    float yCount;
    float gridXsize;
    float gridYsize;

    SpriteRenderer sr;
    FilterGrid grid;
    Bounds bounds;
    Vector3 topLeftPos;
    Vector3 topRightPos;

    Vector3 bottomLeftPos;
    Vector3 bottomRightPos;
    Vector3 moveUpSquare;
    Vector3 moveRightSquare;
    Vector3 moveLeftSquare;
    Vector3 moveDownSquare;


    void OnDrawGizmosSelected()
    {
        Init();
        if (displayGrid)
        {
            DrawGridLines();
            DrawSelectedSquares();
        }
    }

    public void DisplayGizmos() { displayGrid = true; }
    public void HideGizmos() { displayGrid = false; }

    private void SetMovementVectors()
    {
        moveUpSquare = new Vector3(0, gridYsize, 0);
        moveRightSquare = new Vector3(gridXsize, 0, 0);
        moveLeftSquare = new Vector3(-gridXsize, 0, 0);
        moveDownSquare = new Vector3(0, -gridYsize, 0);
    }
    private void Init()
    {
        //currently this function runs periodically find out why and fix it

        sr = GetComponent<SpriteRenderer>();
        grid = GetComponent<FilterGrid>();//add check here if grid was found

        xCount = grid.getWidth();
        yCount = grid.getHeight();

        gridXsize = grid.getGridXSize();
        gridYsize = grid.getGridYSize();

        isIsometric = grid.IsIsometric();


        SetMovementVectors();

        bounds = sr.bounds;

        bottomLeftPos = bounds.min;
        topRightPos = bounds.max;

        bottomRightPos = new Vector3(topRightPos.x, bottomLeftPos.y, 0f);
        topLeftPos = new Vector3(bottomLeftPos.x, topRightPos.y, 0f);

    }


    private void DrawGridLines()
    {
        if (isIsometric)
        {
            DrawIsometricGrid();
        }
        else DrawRectangleGrid();
    }

    private void DrawRectangleGrid()
    {

        for (int i = 0; i <= xCount; i++)
        {
            Gizmos.DrawLine(bottomLeftPos + i * moveRightSquare, topLeftPos + i * moveRightSquare);
        }

        for (int i = 0; i <= yCount; i++)
        {
            Gizmos.DrawLine(bottomLeftPos + i * moveUpSquare, bottomRightPos + i * moveUpSquare);
        }
    }

    public void DrawIsometricGrid()
    {

        //Diagonal one
        Vector3 A = topLeftPos;
        Vector3 B = topLeftPos;

        //Diagonal two
        Vector3 C = topRightPos;
        Vector3 D = topRightPos;

        int i = 0;
        while (i < xCount + yCount - 1)
        {
            if (A.y > bottomLeftPos.y)
            {
                A += moveDownSquare;

            }
            else
            {
                A += moveRightSquare;
            }

            if (B.x < topRightPos.x)
            {
                B += moveRightSquare;
            }
            else
            {
                B += moveDownSquare;
            }


            if (C.x > topLeftPos.x)
            {
                C += moveLeftSquare;
            }
            else
            {
                C += moveDownSquare;
            }


            if (D.y > bottomRightPos.y)
            {
                D += moveDownSquare;
            }
            else
            {
                D += moveLeftSquare;
            }
            Gizmos.DrawLine(A, B);
            Gizmos.DrawLine(C, D);
            i++;
        }
    }




    private void DrawSelectedSquares()
    {
        Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
        Vector3 isoOrigin = new Vector3(
            bounds.center.x,
            bounds.max.y, // center of top tile diamond
            bounds.center.z
        );

        float halfW = gridXsize * 0.5f;
        float halfH = gridYsize * 0.5f;

        for (int y = 0; y < yCount; y++)
        {
            for (int x = 0; x < xCount; x++)
                if (grid.isSelected(x, y))
                {
                    float xCenter = x * halfW - y * halfW;
                    float yCenter = -(y * halfH + x * halfH);

                    Debug.Log("xCenter is " + xCenter);
                    Debug.Log("yCenter is " + yCenter);
                    yCenter += isoOrigin.y;
                    xCenter += isoOrigin.x;

                    Debug.Log("isoOrigin is " + isoOrigin);
                    Gizmos.DrawCube(new Vector3(xCenter, yCenter, -0.1f), new Vector3(gridXsize, gridYsize, 0.1f));//maybe size z should be 1 as now it basically has haight 0
                }

        }
        Gizmos.color = Color.white;
    }



}