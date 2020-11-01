﻿using Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Triggers.Base;
using Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Visitor;
using System;
using System.Linq.Expressions;

namespace Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Triggers.OnInsert
{
    public class OnInsertTriggerActions<TTriggerEntity> : TriggerActions
        where TTriggerEntity : class
    {
        public OnInsertTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnInsertTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        public OnInsertTriggerActions<TTriggerEntity> UpdateAnotherEntity<TUpdateEntity>(
                Expression<Func<TTriggerEntity, TUpdateEntity, bool>> entityFilter,
                Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            ActionExpressions.Add(new OnInsertTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        public override string BuildSql(ITriggerSqlVisitor visitor)
        {
            return visitor.GetTriggerActionsSql(this);
        }
    }
}
