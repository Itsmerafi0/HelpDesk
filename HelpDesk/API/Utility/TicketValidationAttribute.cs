using API.Contracs;
using System.ComponentModel.DataAnnotations;

namespace API.Utility
{
    public class TicketValidationAttribute : ValidationAttribute
    {
        private readonly string _propertyName;

        public TicketValidationAttribute (string propertyName)
        {
            _propertyName = propertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult($"{_propertyName} is required.");
            var ticketRepository = validationContext.GetServices(typeof(ITicketRepository)) as ITicketRepository;

            var checkTicket = ticketRepository.CheckTicket(value.ToString());
            if (checkTicket) return new ValidationResult($"{_propertyName} '{value}' already exists.");
            return ValidationResult.Success;
        }
    }
}
