
using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField]
    PlacementManager placementManager;

[SerializeField]
    InputManager inputManager;

[SerializeField]
    SaveManager saveManager;

    void Awake()
    {
        Init();
    }

    void Start()
    {
        //subscribe to onClick
        inputManager.OnClick += placementManager.Click;
        inputManager.OnSave += saveManager.SaveGame;
        //add esc option that returns the object to previous position
    }


    void Init()
    {

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