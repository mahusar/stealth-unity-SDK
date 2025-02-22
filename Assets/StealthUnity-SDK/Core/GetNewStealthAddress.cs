using Core;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class GetNewStealthAddress : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchNewStealthAddress(string label)
    {
        string response = await rpcHandler.SendRpcRequest("getnewstealthaddress", new object[] { label });

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);

            if (jsonResponse["result"] != null)
            {
                return jsonResponse["result"].ToString();  
            }
            else
            {
                Debug.LogError("Error: " + jsonResponse["error"]?["message"].ToString());
                return "Error: " + jsonResponse["error"]?["message"].ToString();  
            }
        }
        return "Error: Invalid response.";
    }
}

