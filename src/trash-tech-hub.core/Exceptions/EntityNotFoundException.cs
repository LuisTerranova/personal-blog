namespace TrashTechHub.Core.Exceptions;

public class EntityNotFoundException(string entityName, object id)
    : Exception($"{entityName} with ID '{id}' was not found.");
