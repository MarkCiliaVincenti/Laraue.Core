﻿using Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Triggers.Base;
using Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Visitor;
using System;

namespace Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Triggers.OnInsert
{
    public class OnInsertTrigger<TTriggerEntity> : Trigger<TTriggerEntity>
        where TTriggerEntity : class
    {
        public OnInsertTrigger(TriggerTime triggerTime) : base(TriggerType.Insert, triggerTime)
        {
        }

        public OnInsertTrigger<TTriggerEntity> Action(Action<OnInsertTriggerActions<TTriggerEntity>> actionSetup)
        {
            var actionTrigger = new OnInsertTriggerActions<TTriggerEntity>();
            actionSetup.Invoke(actionTrigger);
            Actions.Add(actionTrigger);
            return this;
        }

        public override string BuildSql(ITriggerSqlVisitor visitor)
        {
            return visitor.GetTriggerSql(this);
        }
    }
}
