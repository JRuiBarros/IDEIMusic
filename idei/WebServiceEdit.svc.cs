using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web.Helpers;

namespace idei
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WebServiceEdit" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WebServiceEdit.svc or WebServiceEdit.svc.cs at the Solution Explorer and start debugging.
    public class WebServiceEdit : IWebServiceEdit
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void DoWork()
        {

        }
        public string GetAllRecords()
        {
            return Json.Encode(db.Records);
        }
     
    }
}
