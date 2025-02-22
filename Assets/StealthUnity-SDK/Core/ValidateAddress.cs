using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ValidateAddress : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchValidation(string address)
    {
        string response = await rpcHandler.SendRpcRequest("validateaddress", new object[] { address });

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            var result = jsonResponse["result"];

            if (result != null)
            {
                bool isValid = result["isvalid"]?.Value<bool>() ?? false;
                string validText = isValid ? "Valid" : "Invalid";

                bool isMine = result["ismine"]?.Value<bool>() ?? false;
                bool watchOnly = result["watchonly"]?.Value<bool>() ?? false;
                bool isScript = result["isscript"]?.Value<bool>() ?? false;
                bool isCompressed = result["iscompressed"]?.Value<bool>() ?? false;

                string pubkey = result["pubkey"]?.ToString() ?? "N/A";
                string account = result["account"]?.ToString() ?? "N/A";

                return $"Address: {result["address"]}\n" +
                       $"Status: {validText}\n" +
                       $"Owned by Wallet: {(isMine ? "Yes" : "No")}\n" +
                       $"Watch-Only: {(watchOnly ? "Yes" : "No")}\n" +
                       $"Script Address: {(isScript ? "Yes" : "No")}\n" +
                       $"Public Key: {pubkey}\n" +
                       $"Compressed: {(isCompressed ? "Yes" : "No")}\n" +
                       $"Account: {account}";
            }
        }

        return "Error validating address.";
    }

}

