using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models
{
    public class Discount : AuditableModelBase, IAggregateRoot
    {
        public string Name { get; private set; }
        public double PercentageOff { get; private set; }

        public Discount()
        {

        }

        private Discount(string name, double percentageOff, long userId)
        {
            Name = name;
            PercentageOff = percentageOff;
            Audit(userId, true);
        }

        public static Discount Create(string name, double percentageOff, long userId)
        {
            Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NegativeOrZero(percentageOff, nameof(percentageOff));
            Guard.Against.NegativeOrZero(userId, nameof(userId));
            return new Discount(name, percentageOff, userId);
        }

        public void Update(string name, double percentageOff, long userId)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            PercentageOff = Guard.Against.NegativeOrZero(percentageOff, nameof(percentageOff));
            Guard.Against.NegativeOrZero(userId, nameof(userId));
            Audit(userId, false);
        }
    }
}