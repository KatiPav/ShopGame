using UnityEngine;

public class SideMenu : MonoBehaviour
{
    Animator animator;
    bool isOpen = false;


    public void Awake()
    {
        animator = transform.GetComponent<Animator>();
    }
    public void OnClick()
    {
        Debug.Log("on click called");
        if (isOpen)
        {
            animator.SetTrigger("Close");
        }
        else { animator.SetTrigger("Open"); }
        isOpen = !isOpen;
    }
}