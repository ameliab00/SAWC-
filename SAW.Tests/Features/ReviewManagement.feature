Feature: Review Management

  Scenario: Get reviews for existing event
    When I get reviews for event with ID 2
    Then the response status code should be 200

  Scenario: Create a review for existing event
    When I create a review for event with ID 2 with valid data
    Then the response status code should be 201

  Scenario: Delete an existing review
    When I delete review with ID 7
    Then the response status code should be 200
