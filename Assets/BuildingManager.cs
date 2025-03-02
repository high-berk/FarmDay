using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public GameObject housePrefab; // House prefab
    public GameObject farmPrefab;  // Farm prefab
    public LayerMask groundMask;   // Ground layer mask
    public GameObject cropMenu;    // Crop selection menu (assign in Inspector)
    public GameObject buildMenu;   // Build menu (assign in Inspector)

    public Material validMat;      // Valid placement material (green)
    public Material invalidMat;    // Invalid placement material (red)

    private GameObject previewObject;  // Preview object for building placement
    private GameObject selectedPrefab; // Currently selected prefab (house or farm)
    private bool isValidPlacement = true; // Whether the current placement is valid
    private int gridSize = 2; // Grid size for snapping

    public bool isBuildMode = false; // Whether the player is in Build Mode
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>(); // Occupied grid positions

    void Update()
    {
        if (isBuildMode)
        {
            HandlePreview();   // Show building preview
            HandlePlacement(); // Handle building placement
        }
        else
        {
            HandleFarmInteraction(); // Handle farm interactions (only when NOT in Build Mode)
        }
    }

    void HandlePreview()
    {
        if (selectedPrefab == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundMask))
        {
            Vector3 snapPosition = SnapToGrid(hit.point);

            if (previewObject == null)
            {
                previewObject = Instantiate(selectedPrefab, snapPosition, Quaternion.identity);
                SetPreviewMaterial(validMat);
            }
            else
            {
                previewObject.transform.position = snapPosition;
            }

            isValidPlacement = !occupiedPositions.Contains(snapPosition) && IsValidPlacement(snapPosition);
            SetPreviewMaterial(isValidPlacement ? validMat : invalidMat);
        }
    }

    bool IsValidPlacement(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(
            position,
            new Vector3(gridSize / 2f, 1, gridSize / 2f),
            Quaternion.identity
        );

        foreach (Collider col in colliders)
        {
            if (!col.CompareTag("Ground") && col.gameObject != previewObject)
            {
                return false;
            }
        }
        return true;
    }

    void HandlePlacement()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; // Ignore UI clicks

        if (Input.GetMouseButtonDown(0) && isValidPlacement && selectedPrefab != null)
        {
            Vector3 snapPosition = SnapToGrid(previewObject.transform.position);

            if (!occupiedPositions.Contains(snapPosition))
            {
                Instantiate(selectedPrefab, snapPosition, Quaternion.identity);
                occupiedPositions.Add(snapPosition);
            }
        }
    }

    void HandleFarmInteraction()
    {
        if (isBuildMode) return; // Do nothing if in Build Mode
        if (EventSystem.current.IsPointerOverGameObject()) return; // Ignore UI clicks

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.Log($"Ray hit: {hit.collider.gameObject.name}");

                if (hit.collider.CompareTag("Farm"))
                {
                    Debug.Log("Farm clicked! Collecting fully grown crops.");
                    Crop crop = hit.collider.GetComponent<Crop>();
                    if (crop != null)
                    {
                        crop.CollectFullyGrownCrops(); // Collect fully grown crops
                        OpenCropMenu(hit.collider.gameObject); // Open the crop menu
                    }
                }
            }
        }
    }

    void OpenCropMenu(GameObject farm)
    {
        if (cropMenu != null)
        {
            buildMenu.SetActive(false); // Hide build menu
            cropMenu.SetActive(true);   // Show crop menu

            // Pass the selected farm to the crop menu
            CropMenuHandler cropMenuHandler = cropMenu.GetComponent<CropMenuHandler>();
            if (cropMenuHandler != null)
            {
                cropMenuHandler.SetSelectedFarm(farm);
            }
        }
        else
        {
            Debug.LogError("Crop Menu is not assigned in the Inspector!");
        }
    }

    Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;
        return new Vector3(x, 0, z);
    }

    void SetPreviewMaterial(Material mat)
    {
        if (previewObject == null) return;

        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.material = mat;
        }
    }

    public void SelectBuildingType(bool isHouse)
    {
        selectedPrefab = isHouse ? housePrefab : farmPrefab;
        if (previewObject != null) Destroy(previewObject);
    }

    public void SetBuildMode(bool buildMode)
    {
        isBuildMode = buildMode;

        if (!buildMode && previewObject != null)
        {
            Destroy(previewObject);
            previewObject = null;
        }
    }
}