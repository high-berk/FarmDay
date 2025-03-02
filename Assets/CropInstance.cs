using UnityEngine;

public class CropInstance : MonoBehaviour
{
    public GameObject[] growthStages; // Prefab içindeki büyüme aþamalarý
    public bool isFullyGrown = false; // Is the crop fully grown?

    public void SetGrowthStage(int stage)
    {
        for (int i = 0; i < growthStages.Length; i++)
        {
            growthStages[i].SetActive(i == stage);
        }

        // Check if the crop is fully grown
        if (stage == growthStages.Length - 1)
        {
            isFullyGrown = true;
        }
        else
        {
            isFullyGrown = false;
        }
    }
}