using Microsoft.AspNetCore.Mvc;
using RainfallApi.Data;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace RainfallApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //Operations relating to rainfall
    public class RainfallController : ControllerBase
    {
        private readonly ILogger<RainfallController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        public RainfallController(ILogger<RainfallController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// Get rainfall readings by station Id.
        /// </summary>
        [HttpGet("/rainfall/id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadings(string stationId, int count = 10)
        {
            try
            {
  
                // Specify the URL
                string apiUrl = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?_sorted&_limit={count}";

                // Create an HttpClient using the factory
                var httpClient = _httpClientFactory.CreateClient();

                // Make a request to the specified URL
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                // Check if the request was successful (HTTP 200 OK)
                if (response.IsSuccessStatusCode)
                {
                    // Read the content of the response
                    string jsonData = await response.Content.ReadAsStringAsync();

                    // Parse JSON data using JsonDocument
                    using (JsonDocument doc = JsonDocument.Parse(jsonData))
                    {
                        // Extract readings from the "items" array
                        List<RainfallReading> rainfallReadings = new List<RainfallReading>();
                        JsonElement itemsArray = doc.RootElement.GetProperty("items");

                        foreach (JsonElement item in itemsArray.EnumerateArray())
                        {
                            RainfallReading rainfallReading = new RainfallReading
                            {
                                DateMeasured = DateTime.Parse(item.GetProperty("dateTime").GetString()),
                                AmountMeasured = item.GetProperty("value").GetDouble()
                            };
                            rainfallReadings.Add(rainfallReading);
                        }
                        // Check if there are no readings
                        if (rainfallReadings == null || rainfallReadings.Count == 0)
                        {
                            return NotFound("No readings available");
                        }
                        // Return HTTP 200 OK with the rainfall readings
                        return Ok(new { RainfallReadings = rainfallReadings });
                    }
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    // Return HTTP 404 Not Found if the resource is not found
                    return NotFound("Station not found");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Return HTTP 400 Bad Request if the request is invalid
                    return BadRequest("Invalid request");
                }
                else
                {
                    // Handle other cases where the request was not successful
                    return StatusCode((int)response.StatusCode, "Error retrieving data from the API");
                }
            }
            catch (Exception ex)
            {
                // Return HTTP 500 Internal Server Error for other exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        

    }


  

   



}
