using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;

[Binding]
public class EventManagementSteps
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:5011") };
    private HttpResponseMessage _response;

    [When(@"I request the list of all events")]
    public async Task WhenIRequestTheListOfAllEvents()
    {
        _response = await _client.GetAsync("/api/events");
    }

    [Then(@"the response should be successful")]
    public void ThenTheResponseShouldBeSuccessful()
    {
        Assert.True(_response.IsSuccessStatusCode);
    }

    [When(@"I create a new test event")]
    public async Task WhenICreateANewTestEvent()
    {
        var request = new
        {
            Title = "Test Event",
            Location = "Test Location",
            Price = 100,
            StartingDate = DateTime.UtcNow.AddDays(1),
            EndingDate = DateTime.UtcNow.AddDays(2),
            SeatingCapacity = 50,
            Description = "Test Description"
        };

        var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        _response = await _client.PostAsync("/api/events", content);
    }

    [Then(@"the event should be created successfully")]
    public void ThenTheEventShouldBeCreatedSuccessfully()
    {
        Assert.True(_response.IsSuccessStatusCode);
    }

    [When(@"I update an existing test event")]
    public async Task WhenIUpdateAnExistingTestEvent()
    {
        var startDate = DateTime.UtcNow.Date.AddDays(5);
        var endDate = startDate.AddDays(1);

        var request = new
        {
            Price = 100.0,
            StartingDate = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),  // ISO8601 format
            EndingDate = endDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            SeatingCapacity = 100,
            Description = "Test Description - updated"
        };

        var json = JsonConvert.SerializeObject(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _response = await _client.PatchAsync("/api/events/10", content);
        _response.EnsureSuccessStatusCode();
    }

    [Then(@"the event should be updated successfully")]
    public void ThenTheEventShouldBeUpdatedSuccessfully()
    {
        Assert.True(_response.IsSuccessStatusCode);
    }

    [When(@"I delete an existing test event")]
    public async Task WhenIDeleteAnExistingTestEvent()
    {
        _response = await _client.DeleteAsync("/api/events/10");
    }

    [Then(@"the event should be deleted successfully")]
    public void ThenTheEventShouldBeDeletedSuccessfully()
    {
        Assert.True(_response.IsSuccessStatusCode);
    }
}

