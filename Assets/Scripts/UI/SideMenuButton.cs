using UnityEngine;
public class SideMenuButton : MonoBehaviour
{
    [SerializeField]
    SlidingMenuUI slidingMenuUI;

    [SerializeField]
    SideMenu sideMenu;

    public void Awake()
    {
        if (slidingMenuUI == null)
        {
            Debug.LogError("Side menu button does not have a SlidingMenuUI! Did you forget to reference it in SideMenuButton?");
        }
        if (sideMenu == null)
        {
            Debug.LogError("Side menu button does not have a SideMenu! Did you forget to reference it in SideMenuButton?");
        }
    }

    public void ShowMenu()
    {
        slidingMenuUI.ChangeActiveMenu(sideMenu);

    }

}