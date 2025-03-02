using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public enum CropType { Wheat, Corn } // Crop types

    public GameObject wheatPrefab; // Wheat prefab
    public GameObject cornPrefab;  // Corn prefab

    public List<Transform> cropLocations = new List<Transform>(); // Crop positions
    private List<GameObject> plantedCrops = new List<GameObject>(); // Planted crops

    public float growthTime = 10f; // Growth time
    private Dictionary<GameObject, int> cropStages = new Dictionary<GameObject, int>(); // Growth stages

    // Plant crops of the selected type
    public void PlantCrops(CropType selectedCropType)
    {
        ClearCrops(); // Clear existing crops

        foreach (Transform loc in cropLocations)
        {
            GameObject selectedPrefab = (selectedCropType == CropType.Wheat) ? wheatPrefab : cornPrefab;
            GameObject newCrop = Instantiate(selectedPrefab, loc.position, Quaternion.identity);
            newCrop.transform.SetParent(loc); // Attach crop to location
            plantedCrops.Add(newCrop); // Store reference
            cropStages[newCrop] = 0;  // Initialize growth stage
            StartCoroutine(Grow(newCrop)); // Start growing
        }
    }

    // Coroutine to handle crop growth
    IEnumerator Grow(GameObject crop)
    {
        while (cropStages[crop] < 2) // 3-stage growth (0, 1, 2)
        {
            yield return new WaitForSeconds(growthTime);
            cropStages[crop]++;
            UpdateModel(crop);
        }
    }

    // Update the crop's visual model based on growth stage
    void UpdateModel(GameObject crop)
    {
        CropInstance cropInstance = crop.GetComponent<CropInstance>();
        if (cropInstance != null)
        {
            cropInstance.SetGrowthStage(cropStages[crop]);
        }
    }

    // Clear all planted crops
    public void ClearCrops()
    {
        foreach (GameObject crop in plantedCrops)
        {
            Destroy(crop);
        }
        plantedCrops.Clear();
        cropStages.Clear();
    }

    // Check for fully grown crops and collect them
    public void CollectFullyGrownCrops()
    {
        List<GameObject> cropsToRemove = new List<GameObject>();

        foreach (GameObject crop in plantedCrops)
        {
            CropInstance cropInstance = crop.GetComponent<CropInstance>();
            if (cropInstance != null && cropInstance.isFullyGrown)
            {
                cropsToRemove.Add(crop);
                Destroy(crop); // Remove the crop
            }
        }

        // Remove collected crops from the list
        foreach (GameObject crop in cropsToRemove)
        {
            plantedCrops.Remove(crop);
            cropStages.Remove(crop);
        }
    }
}