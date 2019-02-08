using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Godzilla
{
    public class DocumentAggregate<TEntity>
    {
        #region Init

        public DocumentAggregate(Document<TEntity> document)
        {
            Document = document;
        }

        protected Document<TEntity> Document { get; }

        public Guid Id => Document.Id;
        public TEntity Value => Document.Value;

        #endregion

        #region Events

        protected async Task PublishEvent(object @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Document.Context.NotificationService.PublishEvent(@event, cancellationToken);
        }

        #endregion

        #region Comparsion

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DocumentAggregate<TEntity>))
                return false;

            var doc2 = obj as DocumentAggregate<TEntity>;
            return Document.Equals(doc2);
        }

        public override int GetHashCode()
        {
            return Document.GetHashCode();
        }

        public static bool operator ==(DocumentAggregate<TEntity> left, DocumentAggregate<TEntity> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(DocumentAggregate<TEntity> left, DocumentAggregate<TEntity> right)
        {
            return !(left == right);
        }

        #endregion
    }
}
