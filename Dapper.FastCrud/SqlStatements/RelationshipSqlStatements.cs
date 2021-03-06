﻿namespace Dapper.FastCrud.SqlStatements
{
    using System.Collections.Generic;
    using System.Data;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using Dapper.FastCrud.Configuration.StatementOptions.Aggregated;
    using Dapper.FastCrud.SqlBuilders;

    /// <summary>
    /// SQL statement factory targeting relationships.
    /// </summary>
    /// <typeparam name="TEntity">Target entity type</typeparam>
    internal abstract class RelationshipSqlStatements<TEntity> : ISqlStatements<TEntity>
    {
        private readonly ISqlStatements<TEntity> _mainEntitySqlStatements;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="mainEntitySqlStatements">Proxy for the main entity</param>
        protected RelationshipSqlStatements(ISqlStatements<TEntity> mainEntitySqlStatements)
        {
            _mainEntitySqlStatements = mainEntitySqlStatements;
        }

        /// <summary>
        /// Combines the current instance with a joined entity.
        /// </summary>
        public abstract ISqlStatements<TEntity> CombineWith<TJoinedEntity>(ISqlStatements<TJoinedEntity> joinedEntitySqlStatements);

        /// <summary>
        /// Gets the publicly accessible SQL builder.
        /// </summary>
        public GenericStatementSqlBuilder SqlBuilder => _mainEntitySqlStatements.SqlBuilder;

        /// <summary>
        /// Performs a SELECT operation on a single entity, using its keys
        /// </summary>
        public abstract TEntity SelectById(IDbConnection connection, TEntity keyEntity, AggregatedSqlStatementOptions<TEntity> statementOptions);

        /// <summary>
        /// Performs a SELECT operation on a single entity, using its keys
        /// </summary>
        public abstract Task<TEntity> SelectByIdAsync(IDbConnection connection, TEntity keyEntity, AggregatedSqlStatementOptions<TEntity> statementOptions);

        /// <summary>
        /// Performs a COUNT on a range of items.
        /// </summary>
        public abstract int Count(IDbConnection connection, AggregatedSqlStatementOptions<TEntity> statementOptions);

        /// <summary>
        /// Performs a COUNT on a range of items.
        /// </summary>
        public abstract Task<int> CountAsync(IDbConnection connection, AggregatedSqlStatementOptions<TEntity> statementOptions);

        /// <summary>
        /// Performs a common SELECT 
        /// </summary>
        public abstract IEnumerable<TEntity> BatchSelect(IDbConnection connection, AggregatedSqlStatementOptions<TEntity> statementOptions);

        /// <summary>
        /// Performs a common SELECT 
        /// </summary>
        public abstract Task<IEnumerable<TEntity>> BatchSelectAsync(IDbConnection connection, AggregatedSqlStatementOptions<TEntity> statementoptions);

        /// <summary>
        /// Performs an INSERT operation
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(IDbConnection connection, TEntity entity, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            _mainEntitySqlStatements.Insert(connection, entity, statementOptions);
        }

        /// <summary>
        /// Performs an INSERT operation
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task InsertAsync(IDbConnection connection, TEntity entity, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.InsertAsync(connection, entity, statementOptions);
        }

        /// <summary>
        /// Performs an UPDATE operation on an entity identified by its keys.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool UpdateById(IDbConnection connection, TEntity keyEntity, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.UpdateById(connection, keyEntity, statementOptions);
        }

        /// <summary>
        /// Performs an UPDATE operation on an entity identified by its keys.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<bool> UpdateByIdAsync(IDbConnection connection, TEntity keyEntity, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.UpdateByIdAsync(connection, keyEntity, statementOptions);
        }

        /// <summary>
        /// Performs an UPDATE operation on multiple entities identified by an optional WHERE clause.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BulkUpdate(IDbConnection connection, TEntity entity, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.BulkUpdate(connection, entity, statementOptions);
        }

        /// <summary>
        /// Performs an UPDATE operation on multiple entities identified by an optional WHERE clause.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<int> BulkUpdateAsync(IDbConnection connection, TEntity entity, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.BulkUpdateAsync(connection, entity, statementOptions);
        }

        /// <summary>
        /// Performs a DELETE operation on a single entity identified by its keys.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool DeleteById(IDbConnection connection, TEntity keyEntity, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.DeleteById(connection, keyEntity, statementOptions);
        }

        /// <summary>
        /// Performs a DELETE operation on a single entity identified by its keys.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<bool> DeleteByIdAsync(IDbConnection connection, TEntity keyEntity, AggregatedSqlStatementOptions<TEntity> statementoptions)
        {
            return _mainEntitySqlStatements.DeleteByIdAsync(connection, keyEntity, statementoptions);
        }

        /// <summary>
        /// Performs a DELETE operation using a WHERE clause.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int BulkDelete(IDbConnection connection, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.BulkDelete(connection, statementOptions);
        }

        /// <summary>
        /// Performs a DELETE operation using a WHERE clause.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Task<int> BulkDeleteAsync(IDbConnection connection, AggregatedSqlStatementOptions<TEntity> statementOptions)
        {
            return _mainEntitySqlStatements.BulkDeleteAsync(connection, statementOptions);
        }

        protected IEnumerable<StatementSqlBuilderJoinInstruction> ConstructJoinInstructions(AggregatedSqlStatementOptions<TEntity> statementOptions, params GenericStatementSqlBuilder[] joinedEntitySqlBuilders)
        {
            foreach (var joinedEntitySqlBuilder in joinedEntitySqlBuilders)
            {
                var joinedEntityOptions = statementOptions.RelationshipOptions[joinedEntitySqlBuilder.EntityMapping.EntityType];
                yield return new StatementSqlBuilderJoinInstruction(joinedEntitySqlBuilder, joinedEntityOptions.JoinType, joinedEntityOptions.WhereClause, joinedEntityOptions.OrderClause);
            }
        }
    }
}
