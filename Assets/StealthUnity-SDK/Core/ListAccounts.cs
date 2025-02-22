using Core;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ListAccounts : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchAllAccounts()
    {
        string response = await rpcHandler.SendRpcRequest("listaccounts", new object[] { 1 });

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            List<string> accountList = new List<string>();
            foreach (var account in jsonResponse)
            {
                accountList.Add($"{account.Key}: {account.Value}");
            }
            return string.Join("\n", accountList);
        }

        return "No accounts found.";
    }
}

