﻿namespace TestDataUSBasicLibrary.SourceGeneratorHelpers;
public interface IMapPropertiesForTesting<T>
    where T : class
{
    Dictionary<string, PropertyMapper<T>> GetProperties();
    T CreateNewObject();
}