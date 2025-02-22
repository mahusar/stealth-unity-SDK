using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GetStakerAuthorities : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchStakerAuthorities(string stakerName)
    {
        string response = await rpcHandler.SendRpcRequest("getstakerauthorities", new object[] { stakerName });

        if (!string.IsNullOrEmpty(response))
        {
            try
            {
                JObject jsonResponse = JObject.Parse(response);
                if (jsonResponse["error"] != null && jsonResponse["error"].Type != JTokenType.Null)
                {
                    return $"Error: {jsonResponse["error"].ToString()}";
                }

                return jsonResponse["result"]?.ToString() ?? "No data found!";
            }
            catch
            {
                return "Invalid JSON response!";
            }
        }

        return "Request failed!";
    }
}
