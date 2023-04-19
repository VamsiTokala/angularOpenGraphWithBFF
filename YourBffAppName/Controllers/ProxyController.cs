using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Mvc;

namespace YourBffAppName.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ProxyController : ControllerBase
  {
    private readonly HttpClient _httpClient;
    private readonly HtmlParser _htmlParser;

    public ProxyController(IHttpClientFactory httpClientFactory)
    {
      _httpClient = httpClientFactory.CreateClient();
      _htmlParser = new HtmlParser();
    }

    [HttpGet("{*url}")]
    public async Task<IActionResult> Get(string url)
    {
      var angularAppUrl = "http://localhost:4200/"; // Replace with your Angular app's URL
      string decodedUrl = WebUtility.UrlDecode(url);
      string requestUrl = $"{angularAppUrl}{decodedUrl}";


      //var requestUrl = $"{angularAppUrl}/{url}";

      var response = await _httpClient.GetAsync(requestUrl);

      if (!response.IsSuccessStatusCode)
      {
        return StatusCode((int)response.StatusCode);
      }

      var indexHtmlContent = await response.Content.ReadAsStringAsync();
      var document = await _htmlParser.ParseDocumentAsync(indexHtmlContent);

      // Fetch data for Open Graph tags from your data source
      var data = new
      {
        Title = "Your Title",
        Url = "Your URL",
        Image = "Your Image URL",
        Description = "Your Description"
      };

      // Update Open Graph tags
      AppendMetaTag(document, "og:title", data.Title);
      AppendMetaTag(document, "og:type", "article");
      AppendMetaTag(document, "og:url", data.Url);
      AppendMetaTag(document, "og:image", data.Image);
      AppendMetaTag(document, "og:description", data.Description);

      var updatedIndexHtml = document.ToHtml();

      return Content(updatedIndexHtml, "text/html");
    }

    private void AppendMetaTag(IDocument document, string property, string content)
    {
      var metaElement = document.CreateElement("meta");
      metaElement.SetAttribute("property", property);
      metaElement.SetAttribute("content", content);
      document.Head.AppendChild(metaElement);
    }
  }
}
