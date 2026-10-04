using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SlidingMenuUI : MonoBehaviour
{

    List<SideMenu> menus;
    SideMenu activeMenu;

    Animator animator;
    bool isOpen = false;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Sliding Menu UI has no Animator.");
        }

        menus = GetComponentsInChildren<SideMenu>().ToList();
        DisableMenus();

        if (menus.Count > 0)
        {
            menus[0].gameObject.SetActive(true);
            activeMenu = menus[0];
        }

    }

    public void ChangeActiveMenu(SideMenu menuToActivate)
    {
        activeMenu.gameObject.SetActive(false);
        menuToActivate.gameObject.SetActive(true);
        activeMenu = menuToActivate;

    }

    private void DisableMenus()
    {
        foreach (var menu in menus)
        {
            menu.gameObject.SetActive(false);
        }
    }


    public void ShowMenu()
    {
        if (!isOpen)
        {
            animator.SetBool("isOpen", true);
            isOpen = true;
        }
    }

    public void HideMenu()
    {
        if (isOpen)
        {
            animator.SetBool("isOpen", false);

            isOpen = false;

        }
    }
}