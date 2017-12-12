using System;
using System.Collections.Generic;
using System.Linq;
namespace UserInfo.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using UserInfo.Models;
    using Extensions;

    public class MovieController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Movie/Add", Name = "Add movie for a User")]
        public async Task<HttpResponseMessage> Add(Movie movie)
        {
            var userName = Request.GetHeaderValue("username");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Required request header username is missing");
            }

            var existingUser = await DocumentDBRepository<UserDetails>.GetItemsAsync(d =>
              d.UserName == userName);

            if (existingUser == null || existingUser.FirstOrDefault() == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized User");
            }

            var userId = existingUser.FirstOrDefault().Id;
            existingUser.FirstOrDefault().AddtoMovies(movie);
            await DocumentDBRepository<UserDetails>.UpdateItemAsync(userId, existingUser.FirstOrDefault()).ConfigureAwait(false);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("~/api/Movie/List", Name = "List")]
        public async Task<HttpResponseMessage> List()
        {
            var userName = Request.GetHeaderValue("username");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Required request header username is missing");
            }

            var existingUser = await DocumentDBRepository<UserDetails>.GetItemsAsync(d =>
              d.UserName == userName);

            if (existingUser == null || existingUser.FirstOrDefault() == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized User");
            }

            var userMovies = existingUser.FirstOrDefault().Movies;
            return Request.CreateResponse(HttpStatusCode.OK, userMovies);
        }
    }
}