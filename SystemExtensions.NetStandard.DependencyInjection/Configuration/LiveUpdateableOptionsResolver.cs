﻿using Slim.Resolvers;

namespace System.Configuration;

public sealed class LiveUpdateableOptionsResolver : IDependencyResolver
{
    private static readonly Type optionsType = typeof(LiveUpdateableOptionsWrapper<>);

    public bool CanResolve(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ILiveUpdateableOptions<>))
        {
            return true;
        }

        return false;
    }

    public object Resolve(Slim.IServiceProvider serviceProvider, Type type)
    {
        var typedOptionsType = optionsType.MakeGenericType(type.GetGenericArguments());
        var configurationManager = serviceProvider.GetService<IOptionsManager>();
        return Activator.CreateInstance(typedOptionsType, configurationManager);
    }
}
