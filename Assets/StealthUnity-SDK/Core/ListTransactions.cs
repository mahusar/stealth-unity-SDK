using Core;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ListTransactions : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<List<Transaction>> FetchTransactions(int count = 30)
    {
        string response = await rpcHandler.SendRpcRequest("listtransactions", new object[] { "*", count });

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            return jsonResponse["result"]?.ToObject<List<Transaction>>() ?? new List<Transaction>();
        }

        return new List<Transaction>();
    }
}

[System.Serializable]
public class Transaction
{
    public string account;
    public string address;
    public string category;
    public float amount;
    public int confirmations;
    public string txid;
}

