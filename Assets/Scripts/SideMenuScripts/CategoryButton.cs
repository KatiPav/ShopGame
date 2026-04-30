using UnityEngine;
using UnityEngine.UI;

public class ActiveCategory : MonoBehaviour
{
    Button button;

    [SerializeField]
    CategoryInventory selectedInventory;
    public void Start()
    {
        button = GetComponent<Button>();
    }

    public void SwapSprite()
    {

    }

    void onCategorySelect()
    {

    }
}