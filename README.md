# 🛠️ WSDL Parser Service

<p align="center">
  <img src="https://img.shields.io/badge/C%23-7.3-%23178600">
  <img src="https://img.shields.io/badge/.NET%20Framework-4.8-%239780E5">
</p>

This **WCF (Windows Communication Foundation) Web Service** parses **WSDL documents** to extract detailed information about the web services they describe.

## 🚀 API Reference

### 1. Get Web Service Operations

```csharp
string[] getWsOperations(string url)
```

Retrieves an array of operations provided by a web service at the given WSDL URL.  
The returned data includes each operation’s return types, name, and parameter names and types.

#### Parameters

- **string url**: The URL of the WSDL interface to parse.

### 2. Get Hashed Web Service Operation Info

```csharp
Dictionary<string, OperationInfo> getWsHashOperations(string url)
```

Returns a **dictionary** mapping operation names to `OperationInfo` objects.  
Each `OperationInfo` stores the operation name along with its input/output parameter names and types.

#### Parameters

- **string url**: The URL of the WSDL interface to parse.


### 3. Get Web Service Info

```csharp
WebServiceInfo GetWebServiceInfo(string url)
```

Retrieves a `WebServiceInfo` object that contains the service name and details about all available operations.

#### Parameters

- **string url**: The URL of the WSDL interface to parse.

## 🏆 Credits

- [Hussain Ahamed](http://hussainahamed.blogspot.com/2010/10/reading-wsdl-from-both-webservice-and.html)
- [Mike Hadlow, *Code Rant*](http://mikehadlow.blogspot.com/2006/06/simple-wsdl-object.html)
