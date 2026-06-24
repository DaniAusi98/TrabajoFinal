using System;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Core.Domain.Entities
{
    public interface IValidate
    {
        bool IsValid { get; }
        IList<ValidationFailure> GetErrors();
    }

    // New base entity that only provides identity and basic helpers.
    public abstract class DomainEntity<TKey>
    {
        public TKey Id { get; protected set; }

        protected DomainEntity() { }

        public bool IsTransient()
        {
            return EqualityComparer<TKey>.Default.Equals(Id, default(TKey)!);
        }
    }

    // Backwards-compatible validator-enabled entity. It now inherits from DomainEntity<TKey>.
    public class DomainEntity<TKey, TValidator> : DomainEntity<TKey>, IValidate
        where TValidator : IValidator, new()
    {
        protected TValidator Validator { get; }
        private ValidationResult ValidationResult { get; set; }

        public bool IsValid
        {
            get
            {
                if (ValidationResult == null)
                    Validate();
                return ValidationResult.IsValid;
            }
        }

        protected DomainEntity()
        {
            Validator = new TValidator();
        }

        protected void Validate()
        {
            var context = new ValidationContext<object>(this);
            ValidationResult = Validator.Validate(context);
        }

        public IList<ValidationFailure> GetErrors()
        {
            Validate();
            return ValidationResult.Errors;
        }
    }

    // Convenience non-generic Id (int) base to simplify usage where TKey is int.
    public abstract class DomainEntity : DomainEntity<int>
    {
    }
}
