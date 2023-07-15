using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Models
{
    public class NigerianState : ModelBase, IAggregateRoot
    {
        private NigerianState()
        {

        }

        private NigerianState(long id, string state, IList<string> localGovernmentAreas)
        {
            Id = id;
            State = state;
            LocalGovernmentAreas = localGovernmentAreas;
        }

        public string State { get; private set; }
        public IList<string> LocalGovernmentAreas { get; private set; }

        public static NigerianState Create(long id, string state, IList<string> lgas)
        {
            Guard.Against.NegativeOrZero(id, nameof(id));
            Guard.Against.NullOrWhiteSpace(state, nameof(state));
            Guard.Against.NullOrEmpty(lgas, nameof(lgas));
            return new NigerianState(id, state, lgas);
        }
    }
}