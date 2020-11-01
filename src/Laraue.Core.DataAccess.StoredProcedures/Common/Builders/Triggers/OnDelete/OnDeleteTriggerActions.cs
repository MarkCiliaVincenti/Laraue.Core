﻿using Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Triggers.Base;
using Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Visitor;
using System;
using System.Linq.Expressions;

namespace Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Triggers.OnUpdate
{
    public class OnDeleteTriggerActions<TTriggerEntity> : TriggerActions
        where TTriggerEntity : class
    {
        public OnDeleteTriggerActions<TTriggerEntity> Condition(Expression<Func<TTriggerEntity, bool>> condition)
        {
            ActionConditions.Add(new OnDeleteTriggerCondition<TTriggerEntity>(condition));
            return this;
        }

        public OnDeleteTriggerActions<TTriggerEntity> UpdateAnotherEntity<TUpdateEntity>(
                Expression<Func<TTriggerEntity, TUpdateEntity, bool>> entityFilter,
                Expression<Func<TTriggerEntity, TUpdateEntity, TUpdateEntity>> setValues)
            where TUpdateEntity : class
        {
            ActionExpressions.Add(new OnDeleteTriggerUpdateAction<TTriggerEntity, TUpdateEntity>(entityFilter, setValues));
            return this;
        }

        public override string BuildSql(ITriggerSqlVisitor visitor)
        {
            return visitor.GetTriggerActionsSql(this);
        }
    }
}
