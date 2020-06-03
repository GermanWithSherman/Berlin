using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public class Lib
{
    private static Dictionary<Type, Delegate> _dict = new Dictionary<Type, Delegate>();

    public Lib()
    {
        _dict[typeof(string)] = new Func<string, string>((original) => "düpdüp"+original);
    }

    public static T getCopy<T>(T original)
    {
        Delegate del;
        if (!_dict.TryGetValue(typeof(T), out del))
        {
            var statements = new List<Expression>();

            ParameterExpression paramOriginal = Expression.Parameter(typeof(T));

            ParameterExpression instanceParam = Expression.Variable(typeof(T));
            BinaryExpression createInstance = Expression.Assign(instanceParam, Expression.New(typeof(T)));
            statements.Add(createInstance);

            var asd = typeof(T).GetProperties();

            foreach (var fieldInfo in typeof(T).GetFields())
            {
                foreach(var customAttribute in fieldInfo.CustomAttributes)
                {
                    if(customAttribute.AttributeType == typeof(ModAttribute))
                    {
                        MemberExpression getProperty = Expression.Field(instanceParam, fieldInfo);

                        MemberExpression readValue = Expression.Field(paramOriginal, fieldInfo);//Expression.MakeIndex(paramOriginal, indexerProperty, new[] { Expression.Constant(property.Name) });

                        MethodInfo methodInfo = typeof(Lib).GetMethod("getCopy").MakeGenericMethod(fieldInfo.FieldType);
                        Expression call = Expression.Call(methodInfo, readValue);

                        BinaryExpression assignProperty = Expression.Assign(getProperty, call);
                        statements.Add(assignProperty);
                    }
                }
            }

            var returnStatement = instanceParam;
            statements.Add(returnStatement);

            var body = Expression.Block(instanceParam.Type,new[] { instanceParam }, statements.ToArray());

            var lambda = Expression.Lambda<Func<T, T>>(body, paramOriginal);
            del = lambda.Compile();
            _dict[typeof(T)] = del;
        }
        return ((Func<T, T>)del)(original);
    }
}

public class TestInheritanceBuilder
{
    public string Value1 = "1-Tada";
    [Mod]
    public string Value2 = "2-Lala";

    public TestInheritanceBuilder()
    {
    }

    public TestInheritanceBuilder(string value1, string value2)
    {
        Value1 = value1;
        Value2 = value2;
    }

    public override string ToString()
    {
        return $"{Value1} | {Value2}";
    }
}

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class ModAttribute : Attribute
{

}
