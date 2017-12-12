using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using UserInfo.Models;

namespace UserInfo.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Account/Register", Name = "Register a User")]
        public async Task<HttpResponseMessage> Register(UserDetails model)
        {
            try
            {
                if (model == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Required Param is missing");
                }

                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

                var existingUser = await DocumentDBRepository<UserDetails>.GetItemsAsync(d =>
                d.UserName == model.UserName);

                if (existingUser != null && existingUser.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.Forbidden, "Another user with same username already exists");
                }

                await DocumentDBRepository<UserDetails>.CreateItemAsync(model).ConfigureAwait(false);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Account/Login", Name = "Login a user")]
        public async Task<HttpResponseMessage> Login(UserDetails model)
        {
            try
            {
                if (model == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Required Param is missing");
                }

                if (!ModelState.IsValid)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }

                var existingUser = await DocumentDBRepository<UserDetails>.GetItemsAsync(d =>
                d.UserName == model.UserName && d.Password == model.Password);

               if (existingUser == null || existingUser.FirstOrDefault() == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Login Failed");
                }

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
