using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
    public class RpcHandler : MonoBehaviour
    {
        private static RpcHandler instance;
        private HttpClient client;

        public string rpcUser;
        public string rpcPassword;
        public string rpcUrl;

        public static RpcHandler GetInstance()
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("RpcHandler");
                instance = obj.AddComponent<RpcHandler>();
                DontDestroyOnLoad(obj); 
            }
            return instance;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                LoadRpcSettings();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadRpcSettings()
        {
            rpcUser = PlayerPrefs.GetString("RPC_User", "defaultuser");
            rpcPassword = PlayerPrefs.GetString("RPC_Password", "defaultpassword");
            rpcUrl = PlayerPrefs.GetString("RPC_Url", "http://127.0.0.1:8080/");
        }

        public void SaveRpcSettings(string user, string password, string url)
        {
            rpcUser = user;
            rpcPassword = password;
            rpcUrl = url;

            PlayerPrefs.SetString("RPC_User", rpcUser);
            PlayerPrefs.SetString("RPC_Password", rpcPassword);
            PlayerPrefs.SetString("RPC_Url", rpcUrl);
            PlayerPrefs.Save();
        }

        public async Task<string> SendRpcRequest(string method, object[] parameters = null)
        {
            using (HttpClient client = new HttpClient())
            {
                var auth = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{rpcUser}:{rpcPassword}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);

                RpcRequest request = new RpcRequest(method, parameters ?? new object[] { });
                string requestJson = JsonConvert.SerializeObject(request);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(rpcUrl, content);
                    return await response.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"RPC Error ({method}): " + ex.Message);
                    return null;
                }
            }
        }
    }

    [System.Serializable]
    public class RpcRequest
    {
        public string jsonrpc = "1.0";
        public string id = "1";
        public string method;
        public object[] @params;

        public RpcRequest(string method, object[] parameters = null)
        {
            this.method = method;
            this.@params = parameters ?? new object[] { };
        }
    }
}
