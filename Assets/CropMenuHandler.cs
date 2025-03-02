using UnityEngine;

public class CropMenuHandler : MonoBehaviour
{
    private GameObject selectedFarm; // The farm that was clicked

    // Set the selected farm
    public void SetSelectedFarm(GameObject farm)
    {
        selectedFarm = farm;
    }

    // Called when a crop is selected from the UI menu
    public void SelectCrop(int cropTypeIndex)
    {
        if (selectedFarm != null)
        {
            Crop crop = selectedFarm.GetComponent<Crop>();
            if (crop != null)
            {
                Crop.CropType selectedCropType = (Crop.CropType)cropTypeIndex; // Convert index to CropType
                crop.PlantCrops(selectedCropType); // Plant the selected crop
            }
        }

        // Close the crop menu
        gameObject.SetActive(false);
    }
}