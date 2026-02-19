
using UnityEngine;


public class GameManager : MonoBehaviour
{
    PlacementManager placementManager = null;
    InputManager inputManager = null;

    SaveManager saveManager = null;
    void Awake()
    {
        Init();
    }

    void Start()
    {
        //subscribe to onClick
        inputManager.OnClick += placementManager.Click;
        //add esc option that returns the object to previous position
    }



    void Init()
    {
        placementManager = GetComponent<PlacementManager>();
        inputManager = GetComponent<InputManager>();
        saveManager = GetComponent<SaveManager>();

        if (placementManager == null)
        {
            Debug.Log("PlacementManager not found!");
        }
        if (inputManager == null)
        {
            Debug.Log("InputManager not found!");
        }
        if (saveManager == null)
        {
            Debug.Log("SaveManager not found!");
        }

    }

}