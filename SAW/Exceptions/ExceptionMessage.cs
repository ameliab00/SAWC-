namespace SAW.Exceptions
{
    public static class ExceptionMessage
    {
        public const string DuplicateEventErrorMessage = "This event {0} already exists.";
        public const string EvenNotFoundErrorMessage = "Event not found with id: {0}";
        public const string DuplicateUserErrorMessage = "This user {0} already exists.";
        public const string EmailExistsErrorMessage = "There is an account with that email address: {0}";
        public const string UserNotFoundExceptionMessage = "User not found with id: {0}";
        public const string UsernameNotFoundExceptionMessage = "User not found with username: {0}";
        public const string SeatsNoAvailableExceptionMessage = "No available seats for this event: {0}";
        public const string TicketNotFoundExceptionMessage = "Ticket not found with id: {0}";
        public const string ReviewNotFoundExceptionMessage = "Review not found with id: {0}";
    }
}