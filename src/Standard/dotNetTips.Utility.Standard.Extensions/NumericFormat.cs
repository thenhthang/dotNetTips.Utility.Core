﻿// ***********************************************************************
// Assembly         : dotNetTips.Utility.Standard.Extensions
// Author           : David McCarter
// Created          : 12-27-2020
//
// Last Modified By : David McCarter
// Last Modified On : 12-23-2020
// ***********************************************************************
// <copyright file="NumericFormat.cs" company="David McCarter -  dotNetTips.com">
//     McCarter Consulting (David McCarter)
// </copyright>
// <summary></summary>
// ***********************************************************************


namespace dotNetTips.Utility.Standard.Extensions
{
    /// <summary>
    /// Class NumericFormat.
    /// Implements the <see cref="dotNetTips.Utility.Standard.Enumeration" />.
    /// </summary>
    /// <seealso cref="dotNetTips.Utility.Standard.Enumeration" />
    public class NumericFormat : Enumeration
    {

        /// <summary>
        /// Custom format. Example: $2,147,483,647.00.
        /// </summary>
        public static readonly NumericFormat Currency = new NumericFormat(0, "C");

        /// <summary>
        /// Custom format. Example: 2147483647.
        /// </summary>
        public static readonly NumericFormat Decimal = new NumericFormat(1, "D");

        /// <summary>
        /// Custom format. Example: 2.147484E+009.
        /// </summary>
        public static readonly NumericFormat Exponential = new NumericFormat(2, "E");

        /// <summary>
        /// Custom format. Example: 2147483647.00.
        /// </summary>
        public static readonly NumericFormat FixedPoint = new NumericFormat(3, "F");

        /// <summary>
        /// Custom format. Example: 2147483647.
        /// </summary>
        public static readonly NumericFormat General = new NumericFormat(4, "G");

        /// <summary>
        /// Custom format. Example: 7FFFFFFF.
        /// </summary>
        public static readonly NumericFormat Hexadecimal = new NumericFormat(5, "X");

        /// <summary>
        /// Custom format. Example: 2,147,483,647.00.
        /// </summary>
        public static readonly NumericFormat Number = new NumericFormat(6, "N");

        /// <summary>
        /// Custom format. Example: 214,748,364,700.00%.
        /// </summary>
        public static readonly NumericFormat Percent = new NumericFormat(7, "P");

        /// <summary>
        /// Custom format. Example:  8.988465674311579E+307.
        /// </summary>
        public static readonly NumericFormat RoundTrip = new NumericFormat(8, "R");


        /// <summary>
        /// Prevents a default instance of the <see cref="NumericFormat" /> class from being created.
        /// </summary>
        private NumericFormat() { }


        /// <summary>
        /// Initializes a new instance of the <see cref="NumericFormat" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="displayName">The display name.</param>
        private NumericFormat(int value, string displayName) : base(value, displayName) { }
    }
}
