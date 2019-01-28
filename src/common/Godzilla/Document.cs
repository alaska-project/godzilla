using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Godzilla
{
    public abstract class Document<TEntity>
    {
        private readonly EntityContext _context;
        private TEntity _entity;

        internal Document(EntityContext context, TEntity entity)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _entity = entity;
        }

        public TEntity Value => _entity;

        public async void Delete()
        {
            await _context.Commands.Delete(_entity);
            _entity = default(TEntity);
        }

        public async Task Update()
        {
            _entity = await _context.Commands.Update(_entity);
        }

        public async Task<TChild> AddChild<TChild>(TChild child)
        {
            return await _context.Commands.Add(child, _entity);
        }

        public async Task<IEnumerable<TChild>> AddChildren<TChild>(IEnumerable<TChild> children)
        {
            return await _context.Commands.Add(children, _entity);
        }

        public async Task Rename(string newName)
        {
            await _context.Commands.Rename(_entity, newName);
        }
    }
}
