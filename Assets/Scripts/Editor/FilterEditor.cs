
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FilterGrid))]
public class FilterEditor : Editor
{
    FilterGrid instanceGrid = null;
    bool inEditMode = false;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);

        if (!inEditMode && GUILayout.Button("Edit Filter"))
        {
            inEditMode = true;
            ((FilterGrid)target).GetComponent<GizmoGrid>().DisplayGizmos();

        }

        if (inEditMode && GUILayout.Button("Stop Editing"))
        {
            inEditMode = false;
            ((FilterGrid)target).GetComponent<GizmoGrid>().HideGizmos();
        }


        if (GUILayout.Button("Save Filter to Prefab"))
        {
            SaveGridPrefab();
        }

    }

    void OnSceneGUI()
    {
        instanceGrid = (FilterGrid)target;

        if (!inEditMode) return;

        Event e = Event.current;

        if (e.type != EventType.MouseDown || e.button != 0) //second check is for the right button on mouse
            return;

        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

        Vector3 worldPoint = ray.origin;

        SpriteRenderer sr = instanceGrid.gameObject.GetComponent<SpriteRenderer>();

        float x, y;
        if (instanceGrid.IsIsometric())
        {
            Vector2 xy = WorldToIsoGrid(worldPoint, instanceGrid.getGridXSize(), instanceGrid.getGridYSize());
            x = xy.x;
            y = xy.y;
        }
        else
        {
            x = Mathf.InverseLerp(sr.bounds.min.x, sr.bounds.max.x, worldPoint.x) * instanceGrid.getWidth();
            y = Mathf.InverseLerp(sr.bounds.min.y, sr.bounds.max.y, worldPoint.y) * instanceGrid.getHeight();

        }
        //add check if outside the grid here

        Debug.Log("tryin to click " + Mathf.FloorToInt(x) + " " + Mathf.FloorToInt(y));
        instanceGrid.Click(Mathf.FloorToInt(x), Mathf.FloorToInt(y));
    }

    private void SaveGridPrefab()
    {
        if (instanceGrid == null)
        {
            Debug.Log("Instance not found");
            return;
        }
        GameObject gridPrefabRoot = PrefabUtility.GetCorrespondingObjectFromSource(instanceGrid.gameObject);
        FilterGrid prefabGrid = gridPrefabRoot.GetComponent<FilterGrid>();

        for (int x = 0; x < instanceGrid.getWidth(); x++)
            for (int y = 0; y < instanceGrid.getHeight(); y++)
                if (instanceGrid.isSelected(x, y))
                {
                    prefabGrid.Select(x, y);
                }
                else prefabGrid.Deselect(x, y);

        EditorUtility.SetDirty(prefabGrid);
        PrefabUtility.SavePrefabAsset(gridPrefabRoot);
    }

    void OnDisable()
    {
        //inEditMode = false;
        //((FilterGrid)target).GetComponent<GizmoGrid>().HideGizmos(); //apperantly target is not guaranteed to exist here??? huh???
    }

    //mostly stole this function from chatgpt!!!
    Vector2 WorldToIsoGrid(Vector3 worldPos, float tileWidth, float tileHeight)
    {
        SpriteRenderer sr = instanceGrid.GetComponent<SpriteRenderer>();

        Vector3 isoOrigin = new Vector3( //top-center is 0,0
                sr.bounds.center.x,
                sr.bounds.max.y + tileHeight / 2f,
                sr.bounds.center.z
            );

        float halfW = tileWidth * 0.5f;
        float halfH = tileHeight * 0.5f;

        float a = worldPos.x - isoOrigin.x;
        float b = isoOrigin.y - worldPos.y;

        float x = (a / halfW + b / halfH) * 0.5f;
        float y = (b / halfH - a / halfW) * 0.5f;


        return new Vector2(x, y);
    }

}