﻿// ***********************************************************************
// Assembly         : dotNetTips.Utility.Standard.Tester
// Author           : David McCarter
// Created          : 11-22-2020
//
// Last Modified By : David McCarter
// Last Modified On : 11-22-2020
// ***********************************************************************
// <copyright file="IPersonPlus.cs" company="David McCarter - dotNetTips.com">
//     McCarter Consulting (David McCarter)
// </copyright>
// <summary>Example interface that has implementation.</summary>
// ***********************************************************************
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace dotNetTips.Utility.Standard.Tester.Models
{
    /// <summary>
    /// Interface IPersonPlus
    /// </summary>
    [XmlRoot(ElementName = "IPersonPlus", Namespace = "http://dotNetTips.Utility.Standard.Tester.Models")]
    public interface IPersonPlus : IPerson
    {
        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <value>The age.</value>
        [IgnoreDataMember]
        public TimeSpan Age => this.CalculateAge();

        /// <summary>
        /// Calculates the person's current age.
        /// </summary>
        /// <returns>TimeSpan.</returns>
        private TimeSpan CalculateAge() => DateTimeOffset.UtcNow.Subtract(this.BornOn);
    }
}