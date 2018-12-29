using Oculus.Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SubnauticaGSI.Main;

namespace SubnauticaGSI
{
    public enum PlayerState
    {
        Menu,
        Loading,
        Playing
    }

    public class AuroraController : MonoBehaviour
    {
        private static GameObject controllerGO;

        public static PlayerState state;

        private static string currentSceneName;

        private string jsonlast;

        public static void Load()
        {
            controllerGO = new GameObject("AuroraController");
            controllerGO.AddComponent<AuroraController>();
            controllerGO.AddComponent<SceneCleanerPreserve>();
            DontDestroyOnLoad(controllerGO);

            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            currentSceneName = scene.name;
            Console.WriteLine("Loaded Scene: " + currentSceneName);

            if (currentSceneName.ToLower().Contains("menu"))
                state = PlayerState.Menu;
            if (controllerGO == null)
            {
                controllerGO = new GameObject("AuroraController").AddComponent<AuroraController>().gameObject;
            }
        }

        public void Update()
        {
            UpdateState();
            SerializeJson();
        }

        private void UpdateState()
        {
            if (currentSceneName.ToLower().Contains("menu"))
                state = PlayerState.Menu;
            else if (!uGUI_SceneLoading.IsLoadingScreenFinished || uGUI.main.loading.IsLoading || !uGUI.main)
                state = PlayerState.Loading;
            else
                state = PlayerState.Playing;
        }

        private void SerializeJson()
        {
            string json = JsonConvert.SerializeObject(new GSINode(), Formatting.Indented);

            if (json != jsonlast)
            Send(json);
            jsonlast = json;
        }

        public static void Send(string json)
        {
            try
            {
                int AuroraPort = 9088;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:" + AuroraPort);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
            catch (InvalidCastException e) //ignore Server issues (Aurora closed)
            {
            }
        }

    }
}
