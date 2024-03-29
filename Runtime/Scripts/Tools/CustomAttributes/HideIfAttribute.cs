﻿using UnityEngine;
using System;

namespace KokelSan.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class HideIfAttribute : PropertyAttribute
    {
        public string ComparedPropertyName { get; private set; }
        public object ExpectedValue { get; private set; }

        /// <summary>
        /// Enum comparison, the field will be hidden if comparedProperty equals expectedValue.
        /// </summary>
        /// <param name="enumPropertyName"> Must be the name of an Enum variable </param>
        /// <param name="expectedValue"></param>
        public HideIfAttribute(string enumPropertyName, object expectedValue)
        {
            ComparedPropertyName = enumPropertyName;
            ExpectedValue = expectedValue;
        }
    
        /// <summary>
        /// Bool comparison, the field will be hidden if comparedProperty is True.
        /// </summary>
        /// <param name="booleanPropertyName"> Must be the name of a boolean variable </param>
        public HideIfAttribute(string booleanPropertyName)
        {
            ComparedPropertyName = booleanPropertyName;
            ExpectedValue = true;
        }
    }
}