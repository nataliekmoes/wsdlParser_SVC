using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Schema;

namespace WsOperations
{
    [ServiceContract]
    public interface IService1
    {
        /// <summary>
        /// Returns an array of strings that stores information about each 
        /// operation provided by the service whose WSDL interface is located
        /// at the given url string instance. Each string contained in the array 
        /// consists of the operation name and input/output parameter names and types.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [OperationContract]
        string[] getWsOperations(string url);


        /// <summary>
        /// Returns an OperationInfoCollection, which stores the
        /// operation name and input/output parameter names and types
        /// for each operation provided by the web service whose WSDL interface
        /// is located at the given url string instance.
        /// NOTE: While this method appears simple, it utilizes fairly
        /// complex methods located in the WebServiceInfo class.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [OperationContract]
        Dictionary<string, OperationInfo> getWsHashOperations(string url);



        /// <summary>
        /// Returns a WebServiceInfo object, which contains information
        /// about a web service, which includes information about the 
        /// operations the service provides, among other things.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [OperationContract]
        WebServiceInfo GetWebServiceInfo(string url);
    }



    /// <summary>
    /// Stores the name and type of an operation parameter.
    /// </summary>
    [DataContract]
    public struct Parameter
    {
        /// <summary> The name of the parameter. </summary>
        [DataMember]
        public string Name;

        /// <summary> The data type of the parameter. </summary>
        [DataMember]
        public string Type;

        /// <summary> 
        /// Initializes a Parameter object with the given type and name. 
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="type">Data type</param>
        public Parameter(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }


    /// <summary> 
    /// A KeyedCollection of OperationInfo objects, keyed by operation name. 
    /// </summary>
    [CollectionDataContract]
    public class OperationInfoCollection : KeyedCollection<string, OperationInfo>
    {
        /// <summary>
        /// Initializes an instance of the OperationInfoCollection class.
        /// </summary>
        public OperationInfoCollection() : base() { }

        /// <summary>
        /// Returns the key for the specified OperationInfo item.
        /// </summary>
        /// <param name="operationInfo"></param>
        /// <returns>A string corresponding to the key for OperationInfo argument.</returns>
        protected override string GetKeyForItem(OperationInfo operationInfo)
        {
            return operationInfo.Name;
        }
    }


    /// <summary>
    /// Represents the information corresponding to an OperationContract, including
    /// the Name, Parameters, and Return Type of a method.
    /// </summary>
    [DataContract]
    public class OperationInfo
    {
        private string _name;
        private Parameter[] _inputParameters;
        private Parameter[] _outputParameters;
        private string _portTypeName;
        
        
        /// <summary>
        /// Initializes an instance of the OperationInfo class with the
        /// given name, portTypeName, input parameters, and output parameters.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="portTypeName"></param>
        /// <param name="inputParameters"></param>
        /// <param name="outputParameters"></param>
        public OperationInfo(string name, string portTypeName, Parameter[] inputParameters, Parameter[] outputParameters)
        {
            _name = name;
            _portTypeName = portTypeName;
            _inputParameters = inputParameters;
            _outputParameters = outputParameters;
        }


        /// <summary>
        /// Gives the name of the operation.
        /// </summary>
        [DataMember]
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary> 
        /// Accesses the input parameters of the operation. 
        /// </summary>
        [DataMember]
        public Parameter[] InputParameters
        {
            get => _inputParameters;
            set => _inputParameters = value;
        }

        /// <summary> 
        /// Accesses the output parameters of the operation. 
        /// </summary>
        [DataMember]
        public Parameter[] OutputParameters
        {
            get => _outputParameters;
            set => _outputParameters = value;
        }

        /// <summary>
        /// The name attribute of the portType WSDL element.
        /// </summary>
        [DataMember]
        public string PortTypeName { get => _portTypeName; set => _portTypeName = value; }
    
    }


    /// <summary>
    /// Represents the information related to a web service, including the 
    /// information related to the operations of a certain web service. 
    /// </summary>
    [DataContract]
    public class WebServiceInfo
    {
        private Dictionary<string, OperationInfo> _ops = new Dictionary<string, OperationInfo>();
        private readonly XmlSchemaSet _schemaSet = new XmlSchemaSet();
        private Uri _url;
        private string _serviceName;


        /// <summary>
        /// Initializes an instance of the WebServiceInfo class with the
        /// given the string instance url, which gives the location of the 
        /// WSDL interface of the service.
        /// </summary>
        /// <param name="url"></param>
        public WebServiceInfo(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            _url = new Uri(url);
            GetWebServiceDescription();  // extract and store the name & parameter info for ea. operation           
        }


        /// <summary>
        /// Loads a WSDL file using the given Uri instance to extract 
        /// the neccessary operation information for instantiating 
        /// the WebServiceInfo class.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private void GetWebServiceDescription()
        {
            ServiceDescription serviceDescription;
            XmlTextReader reader = new XmlTextReader(_url.ToString());
            serviceDescription = ServiceDescription.Read(reader);

            _serviceName = serviceDescription.Name;
            ToSingleWSDL(serviceDescription, _url);  // get all imported schemas

            foreach (PortType portType in serviceDescription.PortTypes)
            {
                foreach (Operation operation in portType.Operations)
                {
                    // extract the messagePartNames for the input and out messages
                    string inputMessageName = operation.Messages.Input.Message.Name;
                    string inputMessagePartName = serviceDescription.Messages[inputMessageName].Parts[0].Element.Name;

                    string outputMessageName = operation.Messages.Output.Message.Name;
                    string outputMessagePartName = serviceDescription.Messages[outputMessageName].Parts[0].Element.Name;

                    // extract the input and output parameter names & types
                    Parameter[] inputParameters = GetParameters(inputMessagePartName);
                    Parameter[] outputParameters = GetParameters(outputMessagePartName);

                    // store the operation info 
                    OperationInfo opInfo = new OperationInfo(operation.Name, portType.Name, inputParameters, outputParameters);
                    Ops.Add(operation.Name, opInfo);
                }
            }
        }


        /// <summary>
        /// Extracts the name and type of each parameter corresponding
        /// to the given operation message
        /// </summary>
        /// <param name="messagePartName"></param>
        /// <returns>An array of Parameters corresponding to the given message name</returns>
        private Parameter[] GetParameters(string messagePartName)
        {
            List<Parameter> parameters = new List<Parameter>();
            // get the type and name of a parameter, located in the child elements of the elements corresponding
            // to the input and output messages 
            foreach (XmlSchemaElement schemaElement in _schemaSet.GlobalElements.Values)
            {
                if (schemaElement != null && schemaElement.Name == messagePartName)
                {
                    XmlSchemaComplexType complexType = schemaElement.SchemaType as XmlSchemaComplexType;
                    if (complexType != null)
                    {
                        XmlSchemaSequence sequence = complexType.Particle as XmlSchemaSequence;
                        if (sequence != null)
                        {
                            foreach (XmlSchemaElement childElement in sequence.Items)  // elements contained within sequence
                            {
                                string parameterName = childElement.Name;
                                string parameterType = childElement.SchemaTypeName.Name;
                                parameters.Add(new Parameter(parameterName, parameterType));
                            }
                        }
                    }
                    break;
                }
            }
            return parameters.ToArray();
        }


        /// <summary>
        /// Integrates the imported schemas with the main schemas
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="baseUri"></param>
        private void ToSingleWSDL(ServiceDescription serviceDescription, Uri baseUri)
        {
            foreach (XmlSchema schema in serviceDescription.Types.Schemas)
            {
                if (schema.Items.Count > 0)  // if there are any elements in the schema
                {
                    _schemaSet.Add(schema);
                }
                // get all of the imports and add them to the schema set, along with their complete Uri
                foreach (XmlSchemaObject schemaObject in schema.Includes)
                {
                    if (schemaObject is XmlSchemaImport import)
                    {
                        try
                        {
                            string schemaUri = new Uri(baseUri, import.SchemaLocation).AbsoluteUri;
                            _schemaSet.Add(import.Namespace, schemaUri);
                        }
                        catch (XmlSchemaException) { }
                    }
                }
            }
            // combine all of the imported schemas with non-imported schemas
            _schemaSet.CompilationSettings.EnableUpaCheck = false;
            _schemaSet.Compile();
        }


        /// <summary>
        /// A Uri instance where the WSDL interface of the service is located.
        /// </summary>
        [DataMember]
        public Uri Url { get => _url; set => _url = value; }

        /// <summary>
        /// The name of the service.
        /// </summary>
        [DataMember]
        public string ServiceName { get => _serviceName; set => _serviceName = value; }

        /// <summary>
        /// A Dictionary of OperationInfo instances keyed by operation name.
        /// </summary>
        [DataMember]
        public Dictionary<string, OperationInfo> Ops { get => _ops; set => _ops = value; }

    }

}

