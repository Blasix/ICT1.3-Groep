using Items;
using Patient;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AvatarScene : MonoBehaviour
{
    public GameObject IconContainer;
    public GameObject SelectedIconPrefab; // Reference to the SelectedIcon prefab
    private GameObject selectedIconInstance;
    private ChildDto child;

    void Start()
    {
        int savedAvatarId = PlayerPrefs.GetInt("avatar_ID", -1); // Get the saved avatar ID from PlayerPrefs

        if (savedAvatarId != -1)
        {
            foreach (Transform child in IconContainer.transform)
            {
                if (child.gameObject.GetInstanceID() == savedAvatarId)
                {
                    PlaceSelectedIconInMiddle(child.gameObject);
                    break;
                }
            }
        }
    }

    public void OnAvatarImageClick(GameObject avatarImage)
    {
        int instanceId = avatarImage.GetInstanceID();
        SavePrefabId(instanceId); 
        PlaceSelectedIconInMiddle(avatarImage);
    }

    public void SavePrefabId(int id)
    {
        // Save the prefab ID (this could be to PlayerPrefs, a file, etc.)
        string ChildId = PlayerPrefs.GetString("SelectedChildId");
        string ChildName = PlayerPrefs.GetString("SelectedChildName");
        string TrajectId = PlayerPrefs.GetString("SelectedTrajectId");
        string ArtsName = PlayerPrefs.GetString("ArtsName");
        child = new ChildDto(TrajectId, ArtsName, ChildName, id);

        // Debug log for all attributes of the child item
        ApiClient apiClient = new ApiClient();
        apiClient.UpdateChild(ChildId, child);
        Debug.Log("PrefabId saved: " + id);
        /////hoi
    }


    public void PlaceSelectedIconInMiddle(GameObject avatarImage)
    {
        if (selectedIconInstance == null)
        {
            selectedIconInstance = Instantiate(SelectedIconPrefab, avatarImage.transform);
        }

        // Set the SelectedIcon's position to the avatarImage's center
        selectedIconInstance.transform.SetParent(avatarImage.transform, false);
        selectedIconInstance.transform.localPosition = Vector3.zero;
    }

    public void OnHomeBtn()
    {
       SceneManager.LoadScene("RoadMapScene");
    }
}
