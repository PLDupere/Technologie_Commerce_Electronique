using Microsoft.AspNetCore.Mvc;

namespace Boutique_en_ligne.Controllers
{
    public class HomeController : Controller
    {
        //Get a list of games. https://api.rawg.io/docs/#operation/platforms_list
        private const string ApiKey = "755e1f2dda34491da4ac33116d2608d0"; // API key
        private const string ApiBaseUrl = "https://api.rawg.io/api/games";

        public async Task<ActionResult> GetListOfGames()
        {
            string apiUrl = $"{ApiBaseUrl}?key={ApiKey}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        ViewBag.HttpStatusCode = (int)response.StatusCode;
                        ViewBag.ApiResponse = responseData;
                        return View();
                    }
                    else
                    {
                        ViewBag.HttpStatusCode = (int)response.StatusCode;
                        return Content($"Error: {response.StatusCode}", "text/plain");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.HttpStatusCode = 500; // Internal Server Error
                    return Content($"Exception: {ex.Message}", "text/plain");
                }
            }
        }
    }
}
