using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu; 


    public void ToggleOptionsMenu()
    {
        bool isActive = optionsMenu.activeSelf;
        optionsMenu.SetActive(!isActive);
    }

    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false); 
    }
}
