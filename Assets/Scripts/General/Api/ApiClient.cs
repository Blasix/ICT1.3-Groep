using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;
using System;
using NUnit.Framework.Internal.Execution;
using Patient;

public class ApiClient : MonoBehaviour
{
    private static readonly API_URL _apiUrl = new API_URL();
    public static string apiurl = _apiUrl.apiurl;
    
    
    public async Task Register(string Email, string Password)
    {
        var registerDto = new PostLoginRequestDTO()
        {
            email = Email,
            password = Password
        };

        string json = JsonUtility.ToJson(registerDto);
        Debug.Log("Register JSON: " + json);
        var response = await PerformApiCall($"{apiurl}/account/register", "POST", json);
        Debug.Log("Register Response: " + response);
        if (!response.Contains("400"))
        {
            await Login(Email, Password);
        }
        else
        {
            Debug.LogError("Registration failed with response: " + response);
        }
    }

    public async Task Login(string Email, string Password)
    {
        var loginDto = new PostLoginRequestDTO()
        {
            email = Email,
            password = Password
        };

        string json = JsonUtility.ToJson(loginDto);
        Debug.Log("Login JSON: " + json);

        var response = await PerformApiCall($"{apiurl}/account/login", "POST", json);
        Debug.Log("Login Response: " + response);

        if (response != null)
        {
            if (!response.Contains("401")) //checkt of inloggen gelukt is
            {
                try
                {
                    TokenItem accessToken = JsonConvert.DeserializeObject<TokenItem>(response);
                    PlayerPrefs.SetString("AccessToken", accessToken.AccessToken); //sla access token op in playerprefs
                    PlayerPrefs.SetString("email", Email); //sla email op in playerprefs
                    Debug.Log($"Access token = {accessToken.AccessToken}");
                    //Hier code toevoegen voor het laden van volgende scene
                    SceneManager.LoadScene("OuderKindKeuzeScene");
                }
                catch (Exception ex)
                {
                    Debug.LogError("Failed to deserialize access token: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Login failed with response: " + response);
            }
        }
        else
        {
            Debug.LogError("Login response is null.");
        }
    }

    public async Task<bool> CheckForDuplicateAppointment(string childName, string appointmentName)
    {
        string response = await PerformApiCall($"{apiurl}/appointments/validation/{childName}/{appointmentName}", "GET");
        Debug.Log("CheckForDuplicate Response: " + response);

        bool isDuplicate = false;
        if (!string.IsNullOrEmpty(response))
        {
            isDuplicate = JsonConvert.DeserializeObject<bool>(response);
        }

        return isDuplicate;
    }

    public async Task PostAppointment(AppointmentItem appointmentItem)
    {
        string json = JsonConvert.SerializeObject(appointmentItem);
        var response = await PerformApiCall($"{apiurl}/appointments", "POST", json);
        Debug.Log("PostAfspraak Response: " + response);
    }

    public async Task DeleteAppointment(string childName, string AppointmentName)
    {
        string response = await PerformApiCall($"{apiurl}/appointments/{childName}/{AppointmentName}", "DELETE");
        Debug.Log("DeleteAppointment Response: " + response);
    }

    public async Task<List<AppointmentItem>> GetAppointments(string childName)
    {
        string response = await PerformApiCall($"{apiurl}/appointments/{childName}", "GET");
        List<AppointmentItem> appointmentItemList = JsonConvert.DeserializeObject<List<AppointmentItem>>(response);
        return appointmentItemList;
    }

    public async Task UpdateAppointment(string childId, int step, string statusLevel)
    {
        string response = await PerformApiCall($"{apiurl}/appointments/{childId}/{step}/{statusLevel}", "PUT");
        Debug.Log("UpdateAppointment Response: " + response);

    }

    public static async Task<string> PerformApiCall(string url, string method, string jsonData = null)
    {
        using (UnityWebRequest request = new UnityWebRequest(url, method))
        {
            string token = PlayerPrefs.GetString("AccessToken");
            if (!string.IsNullOrEmpty(jsonData))
            {
                byte[] jsonToSend = Encoding.UTF8.GetBytes(jsonData);
                request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            if (!string.IsNullOrEmpty(token))
            {
                request.SetRequestHeader("Authorization", "Bearer " + token);
            }

            Debug.Log("Sending request to: " + url);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("API call successful: " + request.downloadHandler.text);
                return request.downloadHandler.text;
            }
            else
            {
                Debug.LogError("Error in API call: " + request.error);
                Debug.LogError(request.downloadHandler.text);
                return null;
            }
        }
    }

    public class Level
    {
        public Guid Id { get; set; }
        public Guid TrajectId { get; set; }
        public int Step { get; set; }
        public string Url { get; set; }
        public string Tekst { get; set; }
        public int TotalSteps { get; set; }
    }
    public async Task<List<Level>> GetAllLevels(int step, string trajectId)
    {
        List<Level> appointments = new List<Level>();
        string response = await PerformApiCall($"{apiurl}/Level/{step}/{trajectId}", "GET");
        if (!string.IsNullOrEmpty(response))
        {
            try
            {
                appointments = JsonConvert.DeserializeObject<List<Level>>(response);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"JSON Parsing Error: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("GetAllChildAppointment response is null.");
        }
        return appointments;
    }

    public async Task UpdateChild(string childId, int prefabId)
    {
        ChildDto child = new ChildDto(PlayerPrefs.GetString("SelectedTrajectId"), "", PlayerPrefs.GetString("SelectedChildName"), prefabId);
        string response = await PerformApiCall($"{apiurl}/Child/{childId}", "PUT", JsonUtility.ToJson(child));
        ChildDto childItem = JsonConvert.DeserializeObject<ChildDto>(response);
        PlayerPrefs.SetInt("avatar_ID", prefabId);

        PlayerPrefs.Save();

    }
}


