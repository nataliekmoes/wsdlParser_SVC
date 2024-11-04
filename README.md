# WSDL Parser Service
![Static Badge](https://img.shields.io/badge/C%23-7.3-%23178600)
![Static Badge](https://img.shields.io/badge/.NET%20Framework-4.8-%239780E5)

WCF (Windows Communication Foundation) Web Service that parses WSDL documents to retrieve information about the web services they describe.

## API Reference
### Get Web Service Operations
```C#
string[] getWsOperations(string url)
```
Returns the **return types**, **operation name**, and **parameter types** for all operations offered by a web service, given the string url of its WSDL document. 

#### Parameters
- `string url`: The string url of the WSDL interface to parse.

### Get Hashed Web Service Operation Info
```C#
Dictionary<string, OperationInfo> getWsHashOperations(string url)
```
Returns a **dictionary** of `OperationInfo` objects keyed by operation name, which store the **operation name**, **input/output** **parameter names**, and **types** for each operation provided by the web service whose WSDL interface is located at the given url string instance.

#### Parameters
- `string url`: The string url of the WSDL interface to parse.

### Get Web Service Info
```C#
WebServiceInfo GetWebServiceInfo(string url)
```
Returns a `WebServiceInfo` object, which contains the name and operation information of the web service described by the WSDL interface given.

#### Parameters
- `string url`: The string url of the WSDL interface to parse.

## Credits
- [Hussain Ahamed](http://hussainahamed.blogspot.com/2010/10/reading-wsdl-from-both-webservice-and.html)
- [Mike Hadlow, *Code Rant*](http://mikehadlow.blogspot.com/2006/06/simple-wsdl-object.html)
