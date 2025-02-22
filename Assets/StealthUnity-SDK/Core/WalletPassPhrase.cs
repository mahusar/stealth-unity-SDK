using Core;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class WalletPassPhrase : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<bool> FetchWalletPassPhrase(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            Debug.LogError("Password cannot be empty.");
            return false;
        }

        object[] parameters = { password, 99999999 }; 
        string response = await rpcHandler.SendRpcRequest("walletpassphrase", parameters);

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            return jsonResponse["result"] == null; 
        }

        return false;
    }

    internal Task<bool> UpdateWalletUnlock(string password)
    {
        throw new NotImplementedException();
    }
}
