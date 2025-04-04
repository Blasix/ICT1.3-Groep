using System.Collections.Generic;
using System.Threading.Tasks;
using Items;
using Newtonsoft.Json;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Journal
{
    public class JournalManager : MonoBehaviour
    {
        public GameObject NotePanelGameObject;
        public GameObject itemPrefab;
        public Transform contentParent;
        private List<NoteItem> _notesList;
        public TMP_InputField TitleInputField;
        public TMP_InputField ContentInputField;
        public Button CreateNoteButton;
        public Button BackButton;
        public Button SaveNoteButton;

        
        private string NoteId;
        private string _createOrEdit = "Create";

        void Start()
        {
            NotePanelGameObject.SetActive(false);
            CreateNoteButton.onClick.AddListener(OnNoteCreationButtonClick);
            BackButton.onClick.AddListener(Back);
            SaveNoteButton.onClick.AddListener(CreateNote);
            LoadNotes();
        }
        public void OnNoteCreationButtonClick()
        {
            _createOrEdit = "create";
            TitleInputField.text = "";
            ContentInputField.text = "";
            NotePanelGameObject.SetActive(true);
        }
        
        public void OnBackButtonPressed()
        {
            SceneManager.LoadScene("RoadMapScene");
        }

        private async void LoadNotes()
        {
            ClearContentParent();
            _notesList = await GetNotes();
            AddItems(_notesList);
        }

        private void ClearContentParent()
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
        }

        private async Task<List<NoteItem>> GetNotes()
        {
            _notesList = JsonConvert.DeserializeObject<List<NoteItem>>(
                await ApiClient.PerformApiCall(ApiClient.apiurl + "/note/" + PlayerPrefs.GetString("SelectedChildId"), "GET"));
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
                TMP_Text contentText = newItem.transform.Find("ButtonSelectNote/TMP_TextContent")?.GetComponent<TMP_Text>();
                TMP_Text datetimeText = newItem.transform.Find("ButtonSelectNote/TMP_DateTime")?.GetComponent<TMP_Text>();
                TMP_Text idText = newItem.transform.Find("ButtonSelectNote/TMP_TextId")?.GetComponent<TMP_Text>();

                if (nameText != null)
                {
                    nameText.text = note.Title;
                }
                else
                {
                    Debug.LogError("TMP_TextName not found in itemPrefab");
                }
                if (contentText != null)
                {
                    contentText.text = note.Content;
                }
                else
                {
                    Debug.LogError("TMP_TextContent not found in itemPrefab");
                }
                if (datetimeText != null)
                {
                    datetimeText.text = note.DateTime;
                }
                else
                {
                    Debug.LogError("TMP_DateTime not found in itemPrefab");
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

                // Set up the select button
                Button selectButton = newItem.transform.Find("ButtonSelectNote")?.GetComponent<Button>();
                if (selectButton != null)
                {
                    selectButton.onClick.AddListener(() => OnAppointmentClick(newItem));
                }
                else
                {
                    Debug.LogError("SelectButton not found in itemPrefab");
                }

                newItem.SetActive(true);
            }
        }

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
        public void OnAppointmentClick(GameObject item)
        {
            _createOrEdit = "Edit";
            NotePanelGameObject.SetActive(true);

            // Find the text components
            TMP_Text nameText = item.transform.Find("ButtonSelectNote/TMP_TextTitle")?.GetComponent<TMP_Text>();
            TMP_Text contentText = item.transform.Find("ButtonSelectNote/TMP_TextContent")?.GetComponent<TMP_Text>();
            TMP_Text datetimeText = item.transform.Find("ButtonSelectNote/TMP_DateTime")?.GetComponent<TMP_Text>();
            TMP_Text idText = item.transform.Find("ButtonSelectNote/TMP_TextId")?.GetComponent<TMP_Text>();

            if (nameText != null && contentText != null && datetimeText != null && idText != null)
            {
                // Retrieve the text values
                string title = nameText.text;
                string content = contentText.text;
                string dateTime = datetimeText.text;
                string id = idText.text;

                // Log the text values (or use them as needed)
                Debug.Log($"Title: {title}, Content: {content}, DateTime: {dateTime}, Id: {id}");

                // Set the text content of the TMP_InputField to the note's content
                if (ContentInputField != null)
                {
                    ContentInputField.text = content;
                    TitleInputField.text = title;
                    NoteId = id;
                }
                else
                {
                    Debug.LogError("NoteContentInputField is not assigned in the Inspector");
                }
            }
            else
            {
                Debug.LogError("One or more text components not found in itemPrefab");
            }
        }

        public async void CreateNote()
        {
            if (_createOrEdit == "create")
            {
                Debug.Log("Creating note");
                NoteDto note = new NoteDto(PlayerPrefs.GetString("SelectedChildId"), TitleInputField.text, ContentInputField.text);
                Debug.Log("Note JSON: " + JsonUtility.ToJson(note));
                try
                {
                    await ApiClient.PerformApiCall(ApiClient.apiurl + "/note", "POST", JsonUtility.ToJson(note));
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error creating note: " + e.Message);
                }
                NotePanelGameObject.SetActive(false);
                LoadNotes();
            }
            else
            {
                Debug.Log("Editing note");
                NoteDto note = new NoteDto(PlayerPrefs.GetString("SelectedChildId"), TitleInputField.text, ContentInputField.text);
                string noteJson = JsonConvert.SerializeObject(note);
                await ApiClient.PerformApiCall(ApiClient.apiurl + "/note/" + NoteId, "PUT", noteJson);
                NotePanelGameObject.SetActive(false);
                LoadNotes();
            }
        }

        public void Back()
        {
            NotePanelGameObject.SetActive(false);
        }
    }
    
}