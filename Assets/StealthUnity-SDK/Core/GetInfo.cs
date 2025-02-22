using Core;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GetInfo : MonoBehaviour
{
    private RpcHandler rpcHandler;

    private void Start()
    {
        rpcHandler = RpcHandler.GetInstance();
    }
    public async Task<string> FetchWalletInfo()
    {
        string response = await rpcHandler.SendRpcRequest("getinfo");

        if (!string.IsNullOrEmpty(response))
        {
            JObject jsonResponse = JObject.Parse(response);
            var result = jsonResponse["result"];

            if (result != null)
            {
                string walletInfo = $"Version: {result["version"]}\n" +
                                    $"Protocol Version: {result["protocolversion"]}\n" +
                                    $"Wallet Version: {result["walletversion"]}\n" +
                                    $"Balance: {result["balance"]} XST\n" +
                                    $"New Mint: {result["newmint"]} XST\n" +
                                    $"Stake: {result["stake"]} XST\n" +
                                    $"Blocks: {result["blocks"]}\n" +
                                    $"Block Hash: {result["blockhash"]}\n" +
                                    $"Money Supply: {result["moneysupply"]} XST\n" +
                                    $"Connections: {result["connections"]}\n" +
                                    $"Proxy: {result["proxy"]}\n" +
                                    $"IP: {result["ip"]}\n" +
                                    $"Difficulty: {result["difficulty"]}\n" +
                                    $"Testnet: {result["testnet"]}\n" +
                                    $"Key Pool Oldest: {result["keypoololdest"]}\n" +
                                    $"Key Pool Size: {result["keypoolsize"]}\n" +
                                    $"Pay Transaction Fee: {result["paytxfee"]} XST\n" +
                                    $"Unlocked Until: {result["unlocked_until"]}\n" +
                                    $"Errors: {result["errors"]}";

                return walletInfo; 
            }
        }

        return "Error fetching wallet info.";
    }
}

