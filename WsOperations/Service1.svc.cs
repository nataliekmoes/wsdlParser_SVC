using System.Collections.Generic;

namespace WsOperations
{
    public class Service1 : IService1
    {
        public string[] getWsOperations(string url)
        {
            List<string> operations = new List<string>();
            WebServiceInfo webServiceInfo = new WebServiceInfo(url);

            foreach (KeyValuePair<string, OperationInfo> method in webServiceInfo.Ops)
            {
                List<string> outParamTypes = new List<string>();
                foreach (Parameter parameter in method.Value.OutputParameters)
                {
                    outParamTypes.Add(parameter.Type);
                }

                List<string> inParamTypes = new List<string>();
                foreach (Parameter parameter in method.Value.InputParameters)
                {
                    inParamTypes.Add(string.Format("{0} {1}", parameter.Type, parameter.Name));
                }

                operations.Add(string.Format("{0} {1}({2})",
                    string.Join(", ", outParamTypes), method.Value.Name, string.Join(", ", inParamTypes)));
            }

            return operations.ToArray();
        }


        public Dictionary<string, OperationInfo> getWsHashOperations(string url)
        {
            return new WebServiceInfo(url).Ops;
        }


        public WebServiceInfo GetWebServiceInfo(string url)
        {
            return new WebServiceInfo(url);
        }

    }
}


