using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public BuildingManager buildingManager; // Reference to the BuildingManager
    public bool isHouse; // True for house, false for plant

    private Button button;
    private Color normalColor = Color.white;
    private Color selectedColor = Color.green; // Color when selected

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);

        // Set initial color
        button.image.color = normalColor;
    }

    void OnButtonClick()
    {
        // Tell the BuildingManager which building type to use
        buildingManager.SelectBuildingType(isHouse);

        // Update button colors
        ResetAllButtonColors();
        button.image.color = selectedColor;
    }

    void ResetAllButtonColors()
    {
        // Reset all building buttons to their normal color
        BuildingButton[] buttons = FindObjectsOfType<BuildingButton>();
        foreach (BuildingButton btn in buttons)
        {
            btn.button.image.color = normalColor;
        }
    }
}