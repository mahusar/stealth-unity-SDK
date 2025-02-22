using Core;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class GetAddressesByAccount : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchAddresses(string account)
    {
        string response = await rpcHandler.SendRpcRequest("getaddressesbyaccount", new object[] { account });

        if (string.IsNullOrEmpty(response))
        {
            return "Error: No response from RPC!";
        }

        try
        {
            JObject jsonResponse = JObject.Parse(response);
            JArray addresses = (JArray)jsonResponse["result"];
            return addresses.Count > 0 ? string.Join("\n", addresses) : "No addresses found for this account.";
        }
        catch
        {
            return "Error: Failed to parse response!";
        }
    }
}
