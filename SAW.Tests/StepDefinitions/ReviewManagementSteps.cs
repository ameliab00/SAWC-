using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;


[Binding]
public class ReviewManagementSteps
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:5011") };
    private HttpResponseMessage _response;

    [When(@"I get reviews for event with ID (.*)")]
    public async Task WhenIGetReviewsForEvent(long eventId)
    {
        _response = await _client.GetAsync($"/api/reviews/{eventId}");
    }

    [When(@"I create a review for event with ID (.*) with valid data")]
    public async Task WhenICreateReviewForEventWithValidData(long reviewId)
    {
        var review = new
        {
            Title = "Test Title",
            Content = "Test content",
            Rating = 4
        };

        var json = JsonConvert.SerializeObject(review);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        _response = await _client.PostAsync($"/api/reviews/{reviewId}", content);
    }

    [When(@"I delete review with ID (.*)")]
    public async Task WhenIDeleteReviewWithId(long reviewId)
    {
        _response = await _client.DeleteAsync($"/api/reviews/{reviewId}");
    }

    [Then(@"the response status code should be (.*)")]
    public void ThenTheResponseStatusCodeShouldBe(int expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, (int)_response.StatusCode);
    }
}