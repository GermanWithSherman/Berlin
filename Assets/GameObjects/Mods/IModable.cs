using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public interface IModable
{
    void mod(IModable modable);
    IModable copyDeep();
}

public interface IModableAutofields { }

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]

public class ModExcludeAttribute : Attribute
{

}

public class ModIncludeAttribute : Attribute
{

}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ModableAttribute : Attribute
{
    public enum FieldOptions { OptIn, OptOut }

    public FieldOptions FieldOptIn = FieldOptions.OptIn;

    public ModableAttribute(FieldOptions fieldOptIn)
    {
        FieldOptIn = fieldOptIn;
    }
}

public static class Modable
{
    //https://www.codeproject.com/Articles/503527/Reflection-optimization-techniques
    private static ConcurrentDictionary<Type, Delegate> _copyFunctions = new ConcurrentDictionary<Type, Delegate>();
    private static ConcurrentDictionary<Type, Delegate> _modFunctions = new ConcurrentDictionary<Type, Delegate>();

    private static IEnumerable<FieldInfo> modFields<T>()
    {
        var result = new List<FieldInfo>();

        ModableAttribute.FieldOptions fieldOptions = ModableAttribute.FieldOptions.OptIn;
        var classAttribute = typeof(T).GetCustomAttribute<ModableAttribute>();
        if (classAttribute != null)
            fieldOptions = classAttribute.FieldOptIn;

        foreach (var fieldInfo in typeof(T).GetFields())
        {
            bool copyThisField = false;
            switch (fieldOptions)
            {
                case ModableAttribute.FieldOptions.OptIn:
                    copyThisField = false;
                    break;
                case ModableAttribute.FieldOptions.OptOut:
                    copyThisField = true;
                    break;
            }

            var customExcludeAttribute = fieldInfo.GetCustomAttribute<ModExcludeAttribute>();
            var customIncludeAttribute = fieldInfo.GetCustomAttribute<ModIncludeAttribute>();

            if (customExcludeAttribute != null)
                copyThisField = false;
            else if (customIncludeAttribute != null)
                copyThisField = true;

            if (copyThisField)
                result.Add(fieldInfo);

        }
        return result;
    }

    private static Delegate copyFunctionCreate<T>()
    {
        var statements = new List<Expression>();

        ParameterExpression paramOriginal = Expression.Parameter(typeof(T));

        ParameterExpression instanceParam = Expression.Variable(typeof(T));
        BinaryExpression createInstance = Expression.Assign(instanceParam, Expression.New(typeof(T)));
        statements.Add(createInstance);
       
        foreach(FieldInfo fieldInfo in modFields<T>()) { 
            MemberExpression getProperty = Expression.Field(instanceParam, fieldInfo);

            MemberExpression readValue = Expression.Field(paramOriginal, fieldInfo);

            MethodInfo methodInfo = typeof(Modable).GetMethod("copyDeep", new[] { fieldInfo.FieldType });

            if (methodInfo == null)
            {
                methodInfo = typeof(Modable).GetMethods().Where(mi => mi.Name == "copyDeep" && mi.GetGenericArguments().Any()).First();
            }

            if (methodInfo.IsGenericMethod)
            {
                try
                {
                    methodInfo = methodInfo.MakeGenericMethod(fieldInfo.FieldType);
                }
                catch
                {
                    Debug.LogWarning($"Failed to copy {fieldInfo.Name} ({fieldInfo.FieldType}) automatically");
                    continue;
                }
            }

            Expression call = Expression.Call(methodInfo, readValue);

            BinaryExpression assignProperty = Expression.Assign(getProperty, call);
            statements.Add(assignProperty);
        }

        var returnStatement = instanceParam;
        statements.Add(returnStatement);

        var body = Expression.Block(instanceParam.Type, new[] { instanceParam }, statements.ToArray());

        var lambda = Expression.Lambda<Func<T, T>>(body, paramOriginal);
        return lambda.Compile();
    }

    private static T copyAutofields<T>(T original)
    {
        Delegate del;
        if (!_copyFunctions.TryGetValue(typeof(T), out del))
        {
            del = copyFunctionCreate<T>();
            _copyFunctions[typeof(T)] = del;
        }
        return ((Func<T, T>)del)(original);
    }

    public static T stupidDetour<T>(T original) where T : IModable
    {
        return copyDeep(original);
    }


    public static T copyDeep<T>(T original) where T: IModable
    {
        try
        {
            if (original == null)
                return default;
            if (original is IModableAutofields)
                return copyAutofields(original);
            return (T)original.copyDeep();
        }
        catch(InvalidCastException)
        {
            Debug.LogError($"{typeof(T)} vs {original.copyDeep().GetType()}");
            return default;
        }
    }

    public static DateTime? copyDeep(DateTime? original)
    {
        DateTime? result = original;
        return result;
    }

    public static bool copyDeep(bool original)
    {
        return original;
    }

    public static float copyDeep(float original)
    {
        return original;
    }

    public static int copyDeep(int original)
    {
        return original;
    }

    public static int? copyDeep(int? original)
    {
        return original;
    }

    public static int[] copyDeep(int[] original)
    {
        if (original == null)
            return null;
        return (int[])original.Clone();
    }

    public static long copyDeep(long original)
    {
        return original;
    }

    public static JToken copyDeep(JToken original)
    {
        return original.DeepClone();
    }

    public static string copyDeep(string original)
    {
        return original;
    }

    private static Delegate modFunctionCreate<T>()
    {
        var statements = new List<Expression>();

        ParameterExpression paramOriginal = Expression.Parameter(typeof(T));
        ParameterExpression paramMod = Expression.Parameter(typeof(T));

        
        foreach (FieldInfo fieldInfo in modFields<T>())
        {
            MemberExpression originalProperty = Expression.Field(paramOriginal, fieldInfo);
            MemberExpression modProperty = Expression.Field(paramMod, fieldInfo);

            MethodInfo methodInfo = typeof(Modable).GetMethod("mod", new[] { fieldInfo.FieldType, fieldInfo.FieldType });

            if (methodInfo == null)
            {
                methodInfo = typeof(Modable).GetMethods().Where(mi => mi.Name == "mod" && mi.GetGenericArguments().Any()).First();
            }

            if (methodInfo.IsGenericMethod)
            {
                try
                {
                    methodInfo = methodInfo.MakeGenericMethod(fieldInfo.FieldType);
                }
                catch
                {
                    Debug.LogWarning($"Failed to mod {fieldInfo.Name} ({fieldInfo.FieldType}) automatically");
                    continue;
                }
            }

            if (methodInfo == null)
                Debug.LogError("WTF");

            Expression call = Expression.Call(methodInfo, originalProperty, modProperty);

            BinaryExpression assignProperty = Expression.Assign(originalProperty, call);
            statements.Add(assignProperty);

           

        }

        var returnStatement = paramOriginal;
        statements.Add(returnStatement);

        var body = Expression.Block(paramOriginal.Type, statements.ToArray());

        var lambda = Expression.Lambda<Func<T, T, T>>(body, new[] { paramOriginal, paramMod });
        return lambda.Compile();
    }

    private static T modAutofields<T>(T original, T mod)
    {
        Delegate del;
        if (!_modFunctions.TryGetValue(typeof(T), out del))
        {
            del = modFunctionCreate<T>();
            _modFunctions[typeof(T)] = del;
        }
        return ((Func<T, T, T>)del)(original,mod);
    }

    public static T mod<T>(T original, T mod) where T : IModable
    {
        if (mod == null)
        {
            if (original == null)
                return default;
            return copyDeep(original);
        }


        if (original == null)
            return copyDeep(mod);//(T)mod.copyDeep();

        T originalCopy = copyDeep(original);

        if (original is IModableAutofields)
            return modAutofields(original,mod);

        originalCopy.mod(mod);

        return originalCopy;
    }

    public static DateTime? mod(DateTime? original, DateTime? mod)
    {
        if (mod == null)
            return original;
        return mod;
    }

    public static bool mod(bool original, bool mod)
    {
        return mod;
    }

    public static float mod(float original, float mod)
    {
        return mod;
    }

    public static int mod(int original, int mod)
    {
        return mod;
    }

    public static int[] mod(int[] original, int[] mod)
    {
        return mod;
    }

    public static long mod(long original, long mod)
    {
        return mod;
    }

    public static JToken mod(JToken original, JToken mod)
    {
        JObject originalObject = original as JObject;
        JObject modObject = mod as JObject;

        if (originalObject == null || modObject == null)
            return mod;

        originalObject.Merge(modObject, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });

        return originalObject;
    }

    public static int? mod(int? original, int? mod)
    {
        if (mod == null)
            return original;
        return mod;
    }

    public static string mod(string original, string mod)
    {
        if (String.IsNullOrEmpty(mod))
            return original;
        return mod;
    }
}