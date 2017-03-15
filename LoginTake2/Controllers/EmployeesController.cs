using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LoginTake2.Controllers
{
    public class EmployeesController : ApiController
    {
        [Authorize(Roles="SuperAdmin")]
        public IEnumerable<TBL_EMPLOYEES> Get()
        {
            using (dbNEVSEntities entities = new dbNEVSEntities())
            {
                var x = from n in entities.TBL_EMPLOYEES select n;
                return x.ToList();
            }
        }
    }
}
