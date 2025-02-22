using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GetAccountAddress : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchAccountAddress(string accountName)
    {
        string response = await rpcHandler.SendRpcRequest("getaccountaddress", new object[] { accountName });

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            return jsonResponse["result"]?.ToString() ?? "Error";
        }

        return "Error";
    }
}

