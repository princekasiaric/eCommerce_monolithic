using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.DataContracts;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.Domain.ValueObjects;

namespace Construmart.Core.Domain.Models.ProductAggregate
{
    public class ProductImage : AuditableModelBase, IAggregateRoot
    {
        public long ProductId { get; private set; }
        public File ImageFile { get; private set; }
        public bool IsFeatured { get; private set; }

        private ProductImage()
        {

        }

        private ProductImage(
            string uploadUrl,
            string secureUploadUrl,
            bool isFeatured) : this()
        {
            IsFeatured = isFeatured;
            ImageFile = File.Create(uploadUrl, secureUploadUrl);
        }

        public static ProductImage Create(
            string uploadUrl,
            string secureUploadUrl,
            bool isFeatured
        )
        {
            Guard.Against.NullOrWhiteSpace(uploadUrl, nameof(uploadUrl));
            Guard.Against.NullOrWhiteSpace(secureUploadUrl, nameof(secureUploadUrl));
            return new ProductImage(uploadUrl, secureUploadUrl, isFeatured);
        }
    }
}