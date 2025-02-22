using Core;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class RepairWallet : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> RunRepairWallet()
    {
        string response = await rpcHandler.SendRpcRequest("repairwallet");

        if (!string.IsNullOrEmpty(response))
        {
            try
            {
                JObject jsonResponse = JObject.Parse(response);
                bool checkPassed = jsonResponse["result"]?["wallet check passed"]?.ToObject<bool>() ?? false;

                return checkPassed ? "Wallet check passed" : "Wallet check failed";
            }
            catch
            {
                return "Invalid response from daemon";
            }
        }
        return "No response from daemon";
    }
}

