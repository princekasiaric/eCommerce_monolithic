using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Construmart.Core.Domain.SeedWork
{
    public class EnumerationBase
    {
        private readonly int _value;
        private readonly string _displayName;

        protected EnumerationBase()
        {
        }

        protected EnumerationBase(int value, string displayName)
        {
            _value = value >= 0
                ? value
                : throw new ArgumentException($"{nameof(value)} cannot be less than zero", nameof(value));
            _displayName = displayName ?? throw new ArgumentException(null, nameof(displayName));
        }

        public int Value
        {
            get { return _value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public override string ToString() => DisplayName;

        public static IEnumerable<T> GetAll<T>() where T : EnumerationBase =>
            typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Select(f => f.GetValue(null))
                .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not EnumerationBase otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => _value.GetHashCode();

        public static int AbsoluteDifference(EnumerationBase firstValue, EnumerationBase secondValue)
        {
            if (firstValue == null) throw new ArgumentNullException(nameof(firstValue));
            if (secondValue == null) throw new ArgumentNullException(nameof(secondValue));
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : EnumerationBase
        {
            if (value < 0) throw new ArgumentException("value must be 0 or greater", nameof(value));
            var matchingItem = Parse<T, int>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName, bool ignoreCase = false) where T : EnumerationBase
        {
            if (displayName == null) throw new ArgumentNullException(nameof(displayName));
            var matchingItem = ignoreCase
                ? Parse<T, string>(displayName, "display name", item => item.DisplayName.ToUpper() == displayName.ToUpper())
                : Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
        }

        // public static T FromDisplayNameIgnoreCase<T>(string displayName) where T : EnumerationBase
        // {
        //     if (displayName == null) throw new ArgumentNullException(nameof(displayName));
        //     var matchingItem = _Parse<T, string>(displayName, "display name", item => item.DisplayName.ToUpper() == displayName.ToUpper());
        //     return matchingItem;
        // }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : EnumerationBase => GetAll<T>().FirstOrDefault(predicate);

        public int CompareTo(object other) => Value.CompareTo(((EnumerationBase)other).Value);
    }
}