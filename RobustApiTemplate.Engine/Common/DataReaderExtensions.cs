using System.Data;

namespace RobustApiTemplate.Engine.Common;

public static class DataReaderExtensions
{
    public static T GetValue<T>(this IDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        if (reader.IsDBNull(ordinal))
        {
            return default;
        }

        object value = reader.GetValue(ordinal);
        Type targetType = typeof(T);

        if (value is T typedValue)
        {
            return typedValue;
        }

        try
        {
            if (targetType == typeof(string))
            {
                return (T)(object)Convert.ToString(value);
            }
            if (targetType == typeof(int))
            {
                return (T)(object)Convert.ToInt32(value);
            }
            if (targetType == typeof(bool))
            {
                return (T)(object)Convert.ToBoolean(value);
            }
            if (targetType == typeof(decimal))
            {
                return (T)(object)Convert.ToDecimal(value);
            }
            if (targetType == typeof(double))
            {
                return (T)(object)Convert.ToDouble(value);
            }
            if (targetType == typeof(float))
            {
                return (T)(object)Convert.ToSingle(value);
            }
            if (targetType == typeof(DateTime))
            {
                return (T)(object)Convert.ToDateTime(value);
            }
            if (targetType == typeof(Guid))
            {
                return (T)(object)(value is Guid guid ? guid : Guid.Parse(value.ToString()));
            }
            if (targetType == typeof(byte[]))
            {
                return (T)(object)(value as byte[] ?? throw new InvalidCastException($"Cannot convert {value.GetType()} to byte[]"));
            }

            // For other types, try using Convert.ChangeType
            return (T)Convert.ChangeType(value, targetType);
        }
        catch (Exception ex)
        {
            throw new InvalidCastException($"Cannot convert value '{value}' from type {value.GetType()} to type {typeof(T)} for column '{columnName}'", ex);
        }
    }

    // Convenience methods
    public static string GetString(this IDataReader reader, string columnName) => reader.GetValue<string>(columnName);
    public static int GetInt32(this IDataReader reader, string columnName) => reader.GetValue<int>(columnName);
    public static bool GetBoolean(this IDataReader reader, string columnName) => reader.GetValue<bool>(columnName);
    public static decimal GetDecimal(this IDataReader reader, string columnName) => reader.GetValue<decimal>(columnName);
    public static double GetDouble(this IDataReader reader, string columnName) => reader.GetValue<double>(columnName);
    public static float GetFloat(this IDataReader reader, string columnName) => reader.GetValue<float>(columnName);
    public static DateTime GetDateTime(this IDataReader reader, string columnName) => reader.GetValue<DateTime>(columnName);
    public static Guid GetGuid(this IDataReader reader, string columnName) => reader.GetValue<Guid>(columnName);
    public static byte[] GetByteArray(this IDataReader reader, string columnName) => reader.GetValue<byte[]>(columnName);
}