using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class WalletLock : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<bool> FetchWalletLock()
    {
        string response = await rpcHandler.SendRpcRequest("walletlock");

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            return jsonResponse["result"] == null;
        }
        return false; 
    }
}
