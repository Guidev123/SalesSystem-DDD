using SalesSystem.SharedKernel.DomainObjects;

namespace SalesSystem.Register.Domain.ValueObjects
{
    public record Document : ValueObject
    {
        private const int CPF_MAX_LENGTH = 11;
        public string Number { get; private set; }
        public Document(string number)
        {
            Number = number;
            Validate();
        }
        public static string JustNumbers(string input) => new(input.Where(char.IsDigit).ToArray());

        public static bool IsValid(string number)
        {
            number = JustNumbers(number);

            if (number.Length > CPF_MAX_LENGTH)
                return false;

            while (number.Length != CPF_MAX_LENGTH)
                number = '0' + number;

            var equal = true;
            for (var i = 1; i < CPF_MAX_LENGTH && equal; i++)
                if (number[i] != number[0])
                    equal = false;

            if (equal || number == "12345678909")
                return false;

            var numbers = new int[CPF_MAX_LENGTH];

            for (var i = 0; i < CPF_MAX_LENGTH; i++)
                numbers[i] = int.Parse(number[i].ToString());

            var sum = 0;
            for (var i = 0; i < 9; i++)
                sum += (10 - i) * numbers[i];

            var result = sum % CPF_MAX_LENGTH;

            if (result == 1 || result == 0)
            {
                if (numbers[9] != 0)
                    return false;
            }
            else if (numbers[9] != CPF_MAX_LENGTH - result)
                return false;

            sum = 0;
            for (var i = 0; i < 10; i++)
                sum += (CPF_MAX_LENGTH - i) * numbers[i];

            result = sum % CPF_MAX_LENGTH;

            if (result == 1 || result == 0)
            {
                if (numbers[10] != 0)
                    return false;
            }
            else if (numbers[10] != CPF_MAX_LENGTH - result)
                return false;

            return true;
        }
        public override void Validate()
        {
            AssertionConcern.EnsureTrue(IsValid(Number), "Document is not valid.");
        }
    }
}