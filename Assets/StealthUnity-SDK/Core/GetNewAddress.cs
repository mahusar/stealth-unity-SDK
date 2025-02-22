using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GetNewAddress : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }

    public async Task<string> FetchNewAddress(string account = null)
    {
        object[] parameters = string.IsNullOrEmpty(account) ? new object[] { } : new object[] { account };

        string response = await rpcHandler.SendRpcRequest("getnewaddress", parameters);

        if (string.IsNullOrEmpty(response))
        {
            return "Error: No response from RPC!";
        }

        try
        {
            JObject jsonResponse = JObject.Parse(response);
            return jsonResponse["result"]?.ToString() ?? "Error: No address generated!";
        }
        catch
        {
            return "Error: Failed to parse response!";
        }
    }
}
