﻿using System;
using System.Collections.Generic;

namespace Linqify
{
    /// <summary>
    ///     reusable methods for all request processors
    /// </summary>
    internal class RequestProcessorHelper
    {
        /// <summary>
        ///     All queries have an enum type that specifies the query sub-type;
        ///     This method determines if the type parameter parsed is a string
        ///     or int and performs the conversion to the enum type.
        /// </summary>
        /// <remarks>
        ///     Delphi enums come to the IRequestProcessor as pneumonic strings,
        ///     but C# enums arrive as the underlying int type of the enum;
        ///     therefore, we must determine what we're working with to succeed.
        /// </remarks>
        /// <typeparam name="T">Enum type to convert to</typeparam>
        /// <param name="queryType">
        ///     Either a string enum member name (from Delphi Prism)
        ///     or an underlying int value (from C#/VB)
        /// </param>
        /// <returns>Requested enum type</returns>
        internal static T ParseQueryEnumType<T>(string queryType)
        {
            T statusType;

            if (queryType.GetType() == typeof(string))
            {
                statusType = (T)Enum.Parse(
                    enumType: typeof(T),
                    value: queryType,
                    ignoreCase: true);
            }
            else
            {
                statusType = (T)Enum.ToObject(typeof(T), int.Parse(queryType));
            }

            return statusType;
        }

        /// <summary>
        ///     Determines if a parameter is true
        /// </summary>
        /// <param name="parameters">Name/Value list of parameters</param>
        /// <param name="key">Name of parameter to check.</param>
        /// <returns>Boolean value of parameter (false if value is false or not bool)</returns>
        internal static bool FlagTrue(IDictionary<string, string> parameters, string key)
        {
            bool flag;

            if (!bool.TryParse(parameters[key], out flag))
            {
                flag = false;
            }

            return flag;
        }
    }
}