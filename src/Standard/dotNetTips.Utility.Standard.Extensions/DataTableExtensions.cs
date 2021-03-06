﻿// ***********************************************************************
// Assembly         : dotNetTips.Utility.Standard.Extensions
// Author           : David McCarter
// Created          : 10-08-2020
//
// Last Modified By : David McCarter
// Last Modified On : 10-08-2020
// ***********************************************************************
// <copyright file="DataTableExtensions.cs" company="David McCarter - dotNetTips.com">
//     McCarter Consulting (David McCarter)
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Data;
using dotNetTips.Utility.Standard.Common;

namespace dotNetTips.Utility.Standard.Extensions
{
    /// <summary>
    /// DataTableExtensions.
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Determines whether the specified table has rows.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns><c>true</c> if the specified table has rows; otherwise, <c>false</c>.</returns>
        [Information(nameof(HasRows), author: "David McCarter", createdOn: "10/8/2020", modifiedOn: "10/8/2020", UnitTestCoverage = 0, Status = Status.Available)]
        public static bool HasRows(this DataTable table)
        {
            return (table != null) && (table.Rows != null) && (table.Rows.Count > 0);
        }

        /// <summary>
        /// Determines whether [is database null] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is database null] [the specified value]; otherwise, <c>false</c>.</returns>
        [Information(nameof(HasRows), author: "David McCarter", createdOn: "10/8/2020", modifiedOn: "10/8/2020", UnitTestCoverage = 0, Status = Status.Available)]
        public static bool IsDBNull(object value)
        {
            return Convert.IsDBNull(value);
        }

    }
}
