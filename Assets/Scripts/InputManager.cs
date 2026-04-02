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
            OnClick?.Invoke();
        }
    }

    public void onSavePressed()
    {
        Debug.Log("onSavePressed is called");
        OnSave?.Invoke();
    }
}