using LoginTake2.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoginTake2.Controllers
{
    [RoutePrefix("api/Role")]
    public class RoleController : ApiController
    {

        [Route("RequestAdmin")]
        public string GetRequestAdmin()
        {
            RoleChange obj = new RoleChange();
            return obj.RequestAdminAccess();

        }
        
        [Route("MakeAdmin")]
        public void MakeAdmin()
        {
            RoleChange obj = new RoleChange();
            obj.MakeAdmin();
        }
    }
}
