Feature: Ticket Management

  Scenario: Get tickets for existing event
    When I get tickets for event with ID 2
    Then the ticket response status code should be 200

  Scenario: Get ticket by ID
    When I get ticket by ID 2
    Then the ticket response status code should be 200

  Scenario: Create a ticket for event
    When I create a ticket for event with ID 2
    Then the ticket response status code should be 201

  Scenario: Delete ticket by ID
    When I delete ticket with ID 7
    Then the ticket response status code should be 200
