using System;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

[Binding]
public class TicketManagementSteps
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:5011") };
    private HttpResponseMessage _response;

    [When(@"I get tickets for event with ID (.*)")]
    public async Task WhenIGetTicketsForEventWithID(long eventId)
    {
        _response = await _client.GetAsync($"/api/tickets/{eventId}");
    }

    [When(@"I get ticket by ID (.*)")]
    public async Task WhenIGetTicketByID(long ticketId)
    {
        _response = await _client.GetAsync($"/api/tickets/details/{ticketId}");
    }

    [When(@"I create a ticket for event with ID (.*)")]
    public async Task WhenICreateTicketForEventWithID(long eventId)
    {
        _response = await _client.PostAsync($"/api/tickets/{eventId}", null);
    }

    [When(@"I delete ticket with ID (.*)")]
    public async Task WhenIDeleteTicketWithID(long ticketId)
    {
        _response = await _client.DeleteAsync($"/api/tickets/{ticketId}");
    }

    [Then(@"the ticket response status code should be (.*)")]
    public void ThenTheTicketResponseStatusCodeShouldBe(int expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, (int)_response.StatusCode);
    }
}