using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.Domain.ValueObjects;

namespace Construmart.Core.Domain.Models
{
    public class Category : AuditableModelBase, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsParent { get; private set; }
        public long? ParentCategoryId { get; private set; }

        public Category()
        {

        }

        private Category(string name, bool isActive, bool isParent, long? parentCategoryId, long userId)
        {
            Name = name;
            IsActive = isActive;
            IsParent = isParent;
            ParentCategoryId = parentCategoryId;
            Audit(userId, true);
        }

        public static Category Create(
            string name,
            bool isActive,
            bool isParent,
            long? parentCategoryId,
            long userId)
        {
            Guard.Against.Null(name, nameof(name));
            return new Category(name, isActive, isParent, parentCategoryId, userId);
        }

        public void Update(
            string name,
            bool isActive,
            bool isParent,
            long? parentCategoryId,
            long userId
        )
        {
            Name = Guard.Against.Null(name, nameof(name));
            IsActive = isActive;
            IsParent = isParent;
            ParentCategoryId = parentCategoryId;
            Audit(userId, false);
        }
    }
}