using System;
using TONBRAINS.QUANTON.Grpc.Services;

namespace TONBRAINS.QUANTON.Grpc.Helpers
{
    public static class GrpcGuidHelpers
    {
        public static string ToGrpcValue(this Guid? guid) => guid.HasValue ? guid.ToString() : "";

        public static string ToGrpcValue(this Guid guid) => guid.ToString();

        public static long ToGrpcValue(this DateTime date) => new UnixTimeService().ConvertToLong(date);

        public static long ToGrpcValue(this DateTime? date) => date.HasValue ? new UnixTimeService().ConvertToLong(date.Value) : 0;

        public static bool ToGrpcValue(this bool? value) => value ?? false;

        public static int ToGrpcValue(this int? value) => value ?? 0;

        public static string ToGrpcValue(this string value) => value ?? "";

        public static int ToGrpcValue(this TimeSpan value) => (int)value.TotalSeconds;

        public static int ToGrpcValue(this TimeSpan? timeSpan) => timeSpan.HasValue ? (int)timeSpan.Value.TotalSeconds : 0;

        public static double ToGrpcValue(this double? doubleValue) => doubleValue.HasValue ? doubleValue.Value : 0;

        public static Guid ToGuidValue(this string value) => new Guid(value);

        public static Guid? ToGuidNullableValue(this string value) => string.IsNullOrEmpty(value) ? (Guid?)null : new Guid(value);

        public static DateTime ToDateTimeValue(this long value) => new UnixTimeService().ConvertToDateTime(value);

        public static string DecimalNullableToString(this decimal? value) => value.HasValue ? value.ToString() : "";
    }
}
