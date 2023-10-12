using System.Reflection;

namespace Application.Common.Converting;

public static class Converting
{
    public static object MapDictToObj(Dictionary<string, object> dict, Type destObject)
    {
        var objResult = Activator.CreateInstance( destObject )!;

        foreach (var key in dict.Keys)
        {
            var value = dict[key];

            var targetProperty = destObject.GetProperty( key );
            if (targetProperty == null) continue;
            if (targetProperty.PropertyType == typeof( string ))
            {
                targetProperty.SetValue( objResult, value );
            }
            else
            {
                ParceMetod( targetProperty, value, objResult );
            }
        }

        return objResult;
    }

    private static void ParceMetod(PropertyInfo targetProperty, object value, object returnobj)
    {
        var parseMethod = targetProperty.PropertyType.GetMethod( "TryParse",
            BindingFlags.Public | BindingFlags.Static, null,
            new[] { typeof( string ), targetProperty.PropertyType.MakeByRefType() }, null );

        if (parseMethod == null) return;
        var parameters = new[] { value, null };
        var success = (bool)parseMethod.Invoke( null, parameters )!;
        if (success)
        {
            targetProperty.SetValue( returnobj, parameters[1] );
        }
    }
}