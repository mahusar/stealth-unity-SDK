Stealth (XST) SDK & Wallet UI for Unity engine, providing seamless integration with the StealthCoind daemon. Includes an RPC-powered wallet interface and a modular SDK for developers to use in their Unity projects.

## Stealth Unity SDK
- Lightweight RPC wrapper for Stealth (XST)
- Async JSON-RPC calls (getbalance, getnewaddress, sendtoaddress, etc.)
- Can be used in any Unity project
### Installation
- Option 1: Import StealthUnitySDK.unitypackage into your Unity project
- Option 2: Clone this repository and add it to your Unity project
### Usage
- Add the RpcHandler script to a GameObject in your Unity scene
- Configure your StealthCoin.conf to allow RPC connections

 StealthCoin.conf should include:
- rpcuser=yourusername
- rpcpassword=yourpassword
- rpcallowip=127.0.0.1
- rpcbind=127.0.0.1
- rpcport=xxxx
  
 Use the provided functions to interact with the Stealth network:
- GetNewAddress() – Generates a new XST address
- FetchBalance() – Get the wallet balance
- SendTransaction() – Sends XST to a specified address
- etc.
### Requirements
- Unity 2021.3.45f1 or above
- Newtonsoft.Json 
- StealthCoind daemon v3.2.0.0 
## Stealth Wallet UI
- Fixed 1280x720 windowed wallet application
- UI for balance, transactions, and wallet management
- Direct communication with StealthCoind daemon
- Standalone build

![Alt text](Assets/StealthUnity-SDK/Wallet/Sprites/StealthUnitySDK-Wallet.png)

https://www.youtube.com/watch?v=P87PanqQ6WM 
