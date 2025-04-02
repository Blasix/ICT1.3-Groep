using System.Collections.Generic;
using System.Threading.Tasks;
using Items;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Journal
{
    public class JournalManager : MonoBehaviour
    {
        public void OnNoteCreationButtonClick()
        {
            SceneManager.LoadScene("NoteCreationScene");
        }
        
        public void OnLogoutButtonPressed()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("WelcomeScene");
        }
        
        public GameObject itemPrefab; // Prefab for the list item
        public Transform contentParent; // Content object of the ScrollView
        private List<NoteItem> _notesList;
        void Start()
        {
            LoadNotes();
        }

        private async void LoadNotes()
        {
            _notesList = await GetNotes();
            AddItems(_notesList);
        }

        private async Task<List<NoteItem>> GetNotes()
        {
            _notesList = JsonConvert.DeserializeObject<List<NoteItem>>(
                await ApiClient.PerformApiCall(ApiClient.apiurl + "/note", "GET"));
            return _notesList;
        }

        void AddItems(List<NoteItem> notes)
        {
            // Check if prefab and content parent are assigned
            if (itemPrefab == null || contentParent == null)
            {
                Debug.LogError("Prefab or Content Parent is not assigned!");
                return;
            }
        
            // Loop through the list of appointments
            foreach (var note in notes)
            {
                // Instantiate the prefab
                GameObject newItem = Instantiate(itemPrefab, contentParent);
        
                // Set the text values
                TMP_Text nameText = newItem.transform.Find("ButtonSelectNote/TMP_TextTitle")?.GetComponent<TMP_Text>();
                TMP_Text idText = newItem.transform.Find("ButtonSelectNote/TMP_TextId")?.GetComponent<TMP_Text>();
                
                if (nameText != null)
                {
                    nameText.text = note.Title;
                }
                else
                {
                    Debug.LogError("TMP_TextName not found in itemPrefab");
                }
                if (idText != null)
                {
                    idText.text = note.Id;
                }
                else
                {
                    Debug.LogError("TMP_TextId not found in itemPrefab");
                }
        
                // Set up the delete button
                Button deleteButton = newItem.transform.Find("ButtonDeleteNote")?.GetComponent<Button>();
                if (deleteButton != null)
                {
                    deleteButton.onClick.AddListener(() => DeleteItem(newItem));
                }
                else
                {
                    Debug.LogError("DeleteButton not found in itemPrefab");
                }
                
                // // Set up the select button
                // Button selectButton = newItem.transform.Find("ButtonSelectNote")?.GetComponent<Button>();
                // if (selectButton != null)
                // {
                //     selectButton.onClick.AddListener(() => SelectItem(newItem));
                // }
                // else
                // {
                //     Debug.LogError("SelectButton not found in itemPrefab");
                // }
        
                // Make the new item active
                newItem.SetActive(true);
            }
        }

        // public void SelectItem(GameObject item)
        // {
        //     // Find the text components
        //     TMP_Text nameText = item.transform.Find("ButtonSelectNote/TMP_TextName")?.GetComponent<TMP_Text>();
        //     TMP_Text idText = item.transform.Find("ButtonSelectNote/TMP_TextId")?.GetComponent<TMP_Text>();
        //     
        //     if (idText != null && !string.IsNullOrEmpty(idText.text) && nameText != null && !string.IsNullOrEmpty(nameText.text))
        //     {
        //         // Retrieve the text values
        //         string childName = nameText.text;
        //         string childId = idText.text;
        //     
        //         // Log the text values (or use them as needed)
        //         Debug.Log($"Selected child: {childName} with id: {childId}");
        //         PlayerPrefs.SetString("SelectedChildName", childName);
        //         PlayerPrefs.SetString("SelectedChildId", childId);
        //         
        //         string userType = PlayerPrefs.GetString("UserType");
        //         if (userType == "Ouder")
        //         {
        //             SceneManager.LoadScene("AfsprakenScene");
        //         } else if (userType == "Child")
        //         {
        //             // TODO navigate to child app
        //         } else
        //         {
        //             Debug.LogError("UserType not found in PlayerPrefs");
        //         }
        //     } else
        //     {
        //         Debug.LogError("Id or Name not found in itemPrefab");
        //     }
        // }
        
        private async void SendDeleteRequest(string NoteId)
        {
            await ApiClient.PerformApiCall(ApiClient.apiurl + "/note/" + NoteId, "DELETE");
        }
        
        public void DeleteItem(GameObject item)
        {
            // Find the text components
            TMP_Text idText = item.transform.Find("ButtonSelectNote/TMP_TextId")?.GetComponent<TMP_Text>();
            
            if (idText != null && !string.IsNullOrEmpty(idText.text))
            {
                // Retrieve the text values
                string appointmentName = idText.text;
            
                // Log the text values (or use them as needed)
                SendDeleteRequest(appointmentName);
                Destroy(item);
            } else
            {
                Debug.LogError("Id not found in itemPrefab");
            }
        }
    }
    
}