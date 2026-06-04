namespace TrashTechHub.Core.Exceptions;

public class SlugConflictException(string slug)
    : Exception($"A resource with slug '{slug}' already exists.");
