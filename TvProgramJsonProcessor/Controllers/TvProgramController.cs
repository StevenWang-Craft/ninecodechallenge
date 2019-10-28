using System.Linq;
using System.Net;
using System.Web.Http;
using Newtonsoft.Json;
using TvProgramJsonProcessor.Models;

namespace TvProgramJsonProcessor.Controllers
{
    public class TvProgramController : ApiController
    {
        [Route(""), HttpPost]
        public IHttpActionResult Index()
        {
            using (var contentStream = Request.Content.ReadAsStringAsync())
            {
                try
                {
                    var body = contentStream.Result;
                    var s = JsonConvert.DeserializeObject<RootObject>(body);
                    var result = s.Payload
                        .Where(x => x.Drm && x.EpisodeCount > 0)
                        .Select(x => new TvProgramResponse()
                        {
                            Image = x.Image.ShowImage,
                            Slug = x.Slug,
                            Title = x.Title
                        });
                    return Ok(new successResposneDto { response = result });
                }
                catch
                {
                    var myError = new failureResposneDto
                    {
                        error = "Could not decode request: JSON parsing failed"
                    };
                    return Content(HttpStatusCode.BadRequest, myError);
                }
            }
        }
    }
}
