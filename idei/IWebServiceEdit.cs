using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace idei
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWebServiceEdit" in both code and config file together.
    [ServiceContract]
    public interface IWebServiceEdit
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        string GetAllRecords(string APIKey);
        [OperationContract]
        void newOrder(string newOrder, string APIKey);
        [OperationContract]
        void newSale(string newSale, string APIKey);
    }
}
