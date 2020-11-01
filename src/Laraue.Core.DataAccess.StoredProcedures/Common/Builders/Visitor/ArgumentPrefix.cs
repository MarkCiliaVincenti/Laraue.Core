﻿namespace Laraue.Core.DataAccess.StoredProcedures.Common.Builders.Visitor
{
    /// <summary>
    /// Arguments prefixes for using in SQL generating.
    /// </summary>
    public enum ArgumentPrefix
    {
        /// <summary>
        /// New entity prefix.Available on insert and update operations.
        /// </summary>
        New,

        /// <summary>
        /// Old entity prefix. Available on update and delete operations.
        /// </summary>
        Old,
    }
}
