using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using Xunit;

[Binding]
public class UserManagementSteps
{
    private readonly HttpClient _client = new HttpClient { BaseAddress = new Uri("http://localhost:5011") };
    private HttpResponseMessage _response;

    [When(@"I get the list of users")]
    public async Task WhenIGetTheListOfUsers()
    {
        _response = await _client.GetAsync("/api/users");
    }

    [When(@"I get user by username '(.*)'")]
    public async Task WhenIGetUserByUsername(string username)
    {
        _response = await _client.GetAsync($"/api/users/{username}");
    }

    [When(@"I create a user with username '(.*)', email '(.*)', password '(.*)' and role (.*)")]
    public async Task WhenICreateUser(string username, string email, string password, string role)
    {
        var newUser = new
        {
            UserName = username,
            Email = email,
            Password = password,
            UserRole = Enum.Parse(typeof(SAW.Models.UserRole), role)
        };

        var json = JsonConvert.SerializeObject(newUser);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        _response = await _client.PostAsync("/api/users", content);
    }

    [When(@"I delete user with ID (.*)")]
    public async Task WhenIDeleteUserWithID(long userId)
    {
        _response = await _client.DeleteAsync($"/api/users/{userId}");
    }

    [Then(@"the user response status code should be (.*)")]
    public void ThenTheUserResponseStatusCodeShouldBe(int expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, (int)_response.StatusCode);
    }
}