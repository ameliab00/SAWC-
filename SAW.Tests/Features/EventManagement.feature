Feature: Event Management

  Scenario: Retrieve all events
    When I request the list of all events
    Then the response should be successful

  Scenario: Create a new test event
    When I create a new test event
    Then the event should be created successfully

  Scenario: Update an existing test event
    When I update an existing test event
    Then the event should be updated successfully

  Scenario: Delete an existing test event
    When I delete an existing test event
    Then the event should be deleted successfully
