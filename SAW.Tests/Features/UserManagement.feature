Feature: User Management

  Scenario: Get list of users
    When I get the list of users
    Then the user response status code should be 200

  Scenario: Get user by username
    When I get user by username 'UserTest'
    Then the user response status code should be 200

  Scenario: Create a new user
    When I create a user with username 'newuser', email 'newuser@example.com', password 'password123' and role Participant
    Then the user response status code should be 201

  Scenario: Delete a user by ID
    When I delete user with ID 3
    Then the user response status code should be 200
