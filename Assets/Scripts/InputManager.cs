using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public event Action OnClick;
    public event Action OnSave;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return; // click was consumed by UI, don't propagate to world logic
            }

            OnClick?.Invoke();
        }
    }

    public void onSavePressed()
    {
        Debug.Log("onSavePressed is called");
        OnSave?.Invoke();
    }
}