# Overview

This repository is for documenting and demonstrating [a bug](https://github.com/Azure/azure-functions-openapi-extension/issues/356) in the 1.0.0 version of [Azure/azure-functions-openapi-extension](https://github.com/Azure/azure-functions-openapi-extension).

When attempting to respond with a model that has a recursive object reference there is a side effect where other properties can no longer carry the same type.

For example:

```c#
public sealed class GroupModel
{
    public GroupModel? ChildGroup { get; set; }
    public UserModel? Owner { get; set; }
    public UserModel? CoOwner { get; set; }
}

public sealed class UserModel
{
    public Guid Id { get; set; }
}
```

In the example above, a group can have a child group, an owner, and a co-owner. This leads to the following exception when attempting to generate the Swagger:

```
An item with the same key has already been added. Key: userModel

   at System.Collections.Generic.Dictionary`2.TryInsert(TKey key, TValue value, InsertionBehavior behavior)
   at System.Collections.Generic.Dictionary`2.Add(TKey key, TValue value)
   at System.Linq.Enumerable.ToDictionary[TSource,TKey,TElement](IEnumerable`1 source, Func`2 keySelector, Func`2 elementSelector, IEqualityComparer`1 comparer)
   at System.Linq.Enumerable.ToDictionary[TSource,TKey,TElement](IEnumerable`1 source, Func`2 keySelector, Func`2 elementSelector)
   at Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors.RecursiveObjectTypeVisitor.ProcessProperties(IOpenApiSchemaAcceptor instance, String schemaName, Dictionary`2 properties, NamingStrategy namingStrategy)
   at Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors.RecursiveObjectTypeVisitor.Visit(IAcceptor acceptor, KeyValuePair`2 type, NamingStrategy namingStrategy, Attribute[] attributes)
   at Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Visitors.OpenApiSchemaAcceptor.Accept(VisitorCollection collection, NamingStrategy namingStrategy)
   at Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.DocumentHelper.GetOpenApiSchemas(List`1 elements, NamingStrategy namingStrategy, VisitorCollection collection)
   at Microsoft.Azure.WebJobs.Extensions.OpenApi.Document.Build(Assembly assembly, OpenApiVersionType version)
   at Microsoft.Azure.WebJobs.Extensions.OpenApi.OpenApiTriggerFunctionProvider.RenderSwaggerDocument(HttpRequest req, String extension, ExecutionContext ctx, ILogger log)
```

However, if you remove the `ChildGroup` from the `GroupModel` definition, the exception disappears. You can also get rid of the exception by removing either `Owner` or `CoOwner` from the definition.
