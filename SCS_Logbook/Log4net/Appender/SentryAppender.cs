using log4net.Appender;
using log4net.Core;
using Sentry;
using Sentry.Protocol;
using Sentry.Reflection;
using System;
using System.Collections.Generic;

namespace SCS_Logbook.Log4net.Appender
{
    public class SentryAppender : AppenderSkeleton
    {
        internal static readonly (string Name, string Version) NameAndVersion
            = typeof(SentryAppender).Assembly.GetNameAndVersion();

        private static readonly string ProtocolPackageName = "nuget:" + NameAndVersion.Name;
        
        public string Dsn { get; set; }
        public bool SendIdentity { get; set; }
        public string Environment { get; set; }
        public Level Breadcrumb { get; set; }

        private volatile IDisposable sdkHandle;

        public SentryAppender()
        {
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            if(sdkHandle == null)
            {
                sdkHandle = SentrySdk.Init(o =>
                {
                    o.Dsn = new Dsn(Dsn);
                    o.Debug = true;
                    o.Environment = "dev";
                    o.SendDefaultPii = true;
                });
            }

            if(loggingEvent.Level > Breadcrumb)
            {
                Exception exception = loggingEvent.ExceptionObject ?? loggingEvent.MessageObject as Exception;
                SentryEvent sentryEvent = new SentryEvent(exception)
                {
                    Logger = loggingEvent.LoggerName,
                    Level = loggingEvent.ToSentryLevel()/*,
                    Sdk =
                    {
                        Version = NameAndVersion.Version,
                        Name = "SCS_Logbook.Log4net.Appender.SentryAppender",
                    }*/
                };
                /*
                sentryEvent.Sdk.AddPackage(ProtocolPackageName, NameAndVersion.Version);
                */
                if (!string.IsNullOrWhiteSpace(loggingEvent.RenderedMessage))
                {
                    sentryEvent.Message = loggingEvent.RenderedMessage;
                }

                sentryEvent.SetExtras(GetLoggingEventProperties(loggingEvent));

                if (SendIdentity && !string.IsNullOrEmpty(loggingEvent.Identity))
                {
                    sentryEvent.User = new User
                    {
                        Id = loggingEvent.Identity
                    };
                }

                if (!string.IsNullOrWhiteSpace(Environment))
                {
                    sentryEvent.Environment = Environment;
                }

                SentryId id = SentrySdk.CaptureEvent(sentryEvent);
            }
            else
            {
                SentrySdk.AddBreadcrumb(
                    loggingEvent.RenderedMessage, 
                    loggingEvent.LoggerName, 
                    null,
                    GetLoggingEventExtraInformationString(loggingEvent), 
                    loggingEvent.ToBreadcrumbLevel());
            }
        }

        protected override void OnClose()
        {
            base.OnClose();

            sdkHandle?.Dispose();
        }

        private static IEnumerable<KeyValuePair<string, object>> GetLoggingEventProperties(LoggingEvent loggingEvent)
        {
            var properties = loggingEvent.GetProperties();
            if (properties == null)
            {
                yield break;
            }

            foreach (var key in properties.GetKeys())
            {
                if (!string.IsNullOrWhiteSpace(key)
                    && !key.StartsWith("log4net:", StringComparison.OrdinalIgnoreCase))
                {
                    var value = properties[key];
                    if (value != null
                        && (!(value is string stringValue) || !string.IsNullOrWhiteSpace(stringValue)))
                    {
                        yield return new KeyValuePair<string, object>(key, value);
                    }
                }
            }

            var locInfo = loggingEvent.LocationInformation;
            if (locInfo != null)
            {
                if (!string.IsNullOrEmpty(locInfo.ClassName))
                {
                    yield return new KeyValuePair<string, object>(nameof(locInfo.ClassName), locInfo.ClassName);
                }

                if (!string.IsNullOrEmpty(locInfo.FileName))
                {
                    yield return new KeyValuePair<string, object>(nameof(locInfo.FileName), locInfo.FileName);
                }

                if (int.TryParse(locInfo.LineNumber, out var lineNumber) && lineNumber != 0)
                {
                    yield return new KeyValuePair<string, object>(nameof(locInfo.LineNumber), lineNumber);
                }

                if (!string.IsNullOrEmpty(locInfo.MethodName))
                {
                    yield return new KeyValuePair<string, object>(nameof(locInfo.MethodName), locInfo.MethodName);
                }
            }

            if (!string.IsNullOrEmpty(loggingEvent.ThreadName))
            {
                yield return new KeyValuePair<string, object>(nameof(loggingEvent.ThreadName), loggingEvent.ThreadName);
            }

            if (!string.IsNullOrEmpty(loggingEvent.Domain))
            {
                yield return new KeyValuePair<string, object>(nameof(loggingEvent.Domain), loggingEvent.Domain);
            }

            if (loggingEvent.Level != null)
            {
                yield return new KeyValuePair<string, object>("log4net-level", loggingEvent.Level.Name);
            }
        }

        private static Dictionary<string, string> GetLoggingEventExtraInformationString(LoggingEvent loggingEvent)
        {
            Dictionary<string, string> retval = new Dictionary<string, string>();

            AddPropertiesDict(loggingEvent, ref retval);
            AddLocationInformationDict(loggingEvent, ref retval);

            if (!string.IsNullOrEmpty(loggingEvent.ThreadName))
            {
                retval.Add(nameof(loggingEvent.ThreadName), loggingEvent.ThreadName);
            }

            if (!string.IsNullOrEmpty(loggingEvent.Domain))
            {
                retval.Add(nameof(loggingEvent.Domain), loggingEvent.Domain);
            }

            if (loggingEvent.Level != null)
            {
                retval.Add("log4net-level", loggingEvent.Level.Name);
            }

            return retval;
        }
        
        private static void AddLocationInformationDict(LoggingEvent loggingEvent, ref Dictionary<string, string> retval)
        {
            var locInfo = loggingEvent.LocationInformation;
            if (locInfo != null)
            {
                if (!string.IsNullOrEmpty(locInfo.ClassName))
                {
                    retval.Add(nameof(locInfo.ClassName), locInfo.ClassName);
                }

                if (!string.IsNullOrEmpty(locInfo.FileName))
                {
                    retval.Add(nameof(locInfo.FileName), locInfo.FileName);
                }

                if (int.TryParse(locInfo.LineNumber, out var lineNumber) && lineNumber != 0)
                {
                    retval.Add(nameof(locInfo.LineNumber), lineNumber.ToString());
                }

                if (!string.IsNullOrEmpty(locInfo.MethodName))
                {
                    retval.Add(nameof(locInfo.MethodName), locInfo.MethodName);
                }
            }
        }

        private static void AddPropertiesDict(LoggingEvent loggingEvent, ref Dictionary<string, string> retval)
        {
            var properties = loggingEvent.GetProperties();
            if (properties == null)
            {
                return;
            }

            foreach (string key in properties.GetKeys())
            {
                if (checkKey(key))
                {
                    var value = properties[key];
                    if (checkValue(value))
                    {
                        retval.Add(key, value as string);
                    }
                }
            }
        }

        private static bool checkKey(string key)
        {
            return !string.IsNullOrWhiteSpace(key) && !key.StartsWith("log4net:", StringComparison.OrdinalIgnoreCase);
        }
        
        private static bool checkValue(object value)
        {
            return value != null && (!(value is string stringValue) || !string.IsNullOrWhiteSpace(stringValue));
        }
    }
}
