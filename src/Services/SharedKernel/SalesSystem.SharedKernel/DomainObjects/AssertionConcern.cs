using System.Text.RegularExpressions;

namespace SalesSystem.SharedKernel.DomainObjects
{
    public static class AssertionConcern
    {
        public static void EnsureEqual(object obj1, object obj2, string message)
        {
            if (!obj1.Equals(obj2)) throw new DomainException(message);
        }

        public static void EnsureDifferent(object obj1, object obj2, string message)
        {
            if (obj1.Equals(obj2)) throw new DomainException(message);
        }

        public static void EnsureMaxLength(string value, int max, string message)
        {
            if (value.Trim().Length > max) throw new DomainException(message);
        }

        public static void EnsureLengthInRange(string value, int min, int max, string message)
        {
            var length = value.Trim().Length;
            if (length < min || length > max) throw new DomainException(message);
        }

        public static void EnsureMatchesPattern(string pattern, string value, string message)
        {
            if (!Regex.IsMatch(value, pattern)) throw new DomainException(message);
        }

        public static void EnsureNotEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value)) throw new DomainException(message);
        }

        public static void EnsureNotNull(object obj, string message)
        {
            if (obj is null) throw new DomainException(message);
        }

        public static void EnsureInRange(double value, double min, double max, string message)
        {
            if (value < min || value > max) throw new DomainException(message);
        }

        public static void EnsureInRange(float value, float min, float max, string message)
        {
            if (value < min || value > max) throw new DomainException(message);
        }

        public static void EnsureInRange(int value, int min, int max, string message)
        {
            if (value < min || value > max) throw new DomainException(message);
        }

        public static void EnsureInRange(long value, long min, long max, string message)
        {
            if (value < min || value > max) throw new DomainException(message);
        }

        public static void EnsureInRange(decimal value, decimal min, decimal max, string message)
        {
            if (value < min || value > max) throw new DomainException(message);
        }

        public static void EnsureGreaterThan(long value, long min, string message)
        {
            if (value <= min) throw new DomainException(message);
        }

        public static void EnsureGreaterThan(int value, int min, string message)
        {
            if (value <= min) throw new DomainException(message);
        }

        public static void EnsureGreaterThan(decimal value, decimal min, string message)
        {
            if (value <= min) throw new DomainException(message);
        }

        public static void EnsureGreaterThan(double value, double min, string message)
        {
            if (value <= min) throw new DomainException(message);
        }

        public static void EnsureFalse(bool value, string message)
        {
            if (value) throw new DomainException(message);
        }

        public static void EnsureTrue(bool value, string message)
        {
            if (!value) throw new DomainException(message);
        }
    }
}
