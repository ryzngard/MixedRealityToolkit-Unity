// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace Microsoft.MixedReality.Toolkit.Core.Definitions.Utilities
{
    /// <summary>
    /// An enumeration for controlling the types (and often the volume) of diagnostic
    /// log messages that are emitted from Mixed Reality Toolkit components.
    /// </summary>
    [Flags]
    public enum LoggingLevels
    {
        /// <summary>
        /// Informational messages.
        /// </summary>
        Informational = 1 << 0,     // Hex: 0x00000001, Decimal: 1

        /// <summary>
        /// Assertion messages.
        /// </summary>
        Assert = 1 << 1,            // Hex: 0x00000002, Decimal: 2

        /// <summary>
        /// Warning messages.
        /// </summary>
        Warning = 1 << 2,           // Hex: 0x00000004, Decimal: 4

        /// <summary>
        /// Error messages.
        /// </summary>
        Error = 1 << 3,             // Hex: 0x00000008, Decimal: 8

        /// <summary>
        /// Critical / foundational error messages, generally results in a significant loss 
        /// of functionality or an application exception.
        /// </summary>
        CriticalError = 1 << 4,     // Hex: 0x00000010, Decimal: 16
    }

    public interface ILogger 
    {
        void Assert(bool condition);
        void Assert(bool condition, string message);
        void LogCriticalError(string message);
        void LogInformation(string message);
        void LogWarning(string message);
    }

    public class DebugLogger : ILogger 
    {
        LoggingLevels loggingLevels;
        public DebugLogger(LoggingLevels currentLevel)
        {
            loggingLevels = currentLevel;
        }

        public void Assert(bool condition)
        {
             Assert(condition, string.Empty);
        }

        public void Assert(bool condition, string message)
        {
            if (isEnabled(LoggingLevels.Assert))
            {
                Debug.Assert(condition, message);
            }
        }

        public void LogCriticalError(string message)
        {
            if (isEnabled(LoggingLevels.CriticalError))
            {
                Debug.LogError(message);
            }
        }

        public void LogInformation(string message)
        {
            if (isEnabled(LoggingLevels.Informational))
            {
                Debug.Log(message);
            }
        }

        public void LogWarning(string message)
        {
            if (isEnabled(LoggingLevels.Error))
            {
                Debug.LogWarning(message);
            }
        }

        private bool isEnabled(LoggingLevels levels)
        {
            return (loggingLevels & levels)  == levels;
        }
    }
}