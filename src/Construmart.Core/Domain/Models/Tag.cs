using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models
{
    public class Tag : AuditableModelBase, IAggregateRoot
    {
        public string Name { get; private set; }

        public Tag()
        {

        }

        private Tag(string name, long userId)
        {
            Name = Guard.Against.Null(name, nameof(name));
            Audit(userId, true);
        }

        public static Tag Create(string name, long userId) => new(name, userId);

        public void Update(string name, long userId)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Guard.Against.NegativeOrZero(userId, nameof(userId));
            Audit(userId, false);
        }
    }
}