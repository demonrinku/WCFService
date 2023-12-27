using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IZAQ" in both code and config file together.
    [ServiceContract]
    public interface IZAQ
    {
        [OperationContract]//making the below available as part for a service to client of wxf service
        //[WebGet(ResponseFormat = WebMessageFormat.Json)]
        User GetUser();
    }
}
