# wsdlParser_SVC
WCF (Windows Communication Foundation) Web Service that parses WSDL documents to retrieve information about the web services they describe.


## API Reference

#### `string[] getWsOperations(string url)`

Returns the return types, operation name, and parameter types for all operations offered by a web service, given the url of its WSDL document. Each string in the returned array corresponds to one operation and is of the form: `<return_types> <operation_name>(<parameter_types>)`

<br/>

#### `Dictionary<string, OperationInfo> getWsHashOperations(string url)`

<br/>

#### `WebServiceInfo GetWebServiceInfo(string url)`

<br/>

## Limitations

## Specs
**.NET Framework 4.8**&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;**C# 7.3**

## Credit 
[Hussain Ahamed](http://hussainahamed.blogspot.com/2010/10/reading-wsdl-from-both-webservice-and.html)

[Mike Hadlow, *Code Rant*](http://mikehadlow.blogspot.com/2006/06/simple-wsdl-object.html)
