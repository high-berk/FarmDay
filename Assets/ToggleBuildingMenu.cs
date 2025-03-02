using UnityEngine;

public class ToggleBuildingMenu : MonoBehaviour
{
    public GameObject buildingMenu; // Reference to the Building Menu Panel
    public BuildingManager buildingManager; // Reference to the BuildingManager

    void Start()
    {
        // Hide the building menu at the start
        if (buildingMenu != null)
        {
            buildingMenu.SetActive(false);
        }
    }

    public void OnBuildButtonClick()
    {
        // Toggle the building menu's visibility
        if (buildingMenu != null)
        {
            bool isMenuVisible = buildingMenu.activeSelf;
            buildingMenu.SetActive(!isMenuVisible);

            // Enable/disable Build Mode
            if (buildingManager != null)
            {
                buildingManager.SetBuildMode(!isMenuVisible);
            }
        }
    }
}