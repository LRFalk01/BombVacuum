using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BombVacuum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace BombVacuum.WebApi.Controllers
{
    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [Route("Testing")]
        public IHttpActionResult GetTesting()
        {
            return Ok("Test is a test");
        }

        [Route("Register")]
        public async Task<IHttpActionResult> GetRegister()
        {
            //just making sure the identity stack is running
            var manager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = new ApplicationUser { UserName = "LRFalk01@gmail.com", Email = "LRFalk01@gmail.com" };

            IdentityResult result = await manager.CreateAsync(user, "asdf");

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}