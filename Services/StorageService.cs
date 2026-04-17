using Repositories;
using Services.DTOs;
using StorageModels.Entities;

namespace Services;

public class StorageService : IStorageService
{
    private readonly IStorageRepository _storageRepository;

    public StorageService(IStorageRepository storageRepository)
    {
        _storageRepository = storageRepository;
    }

 
}
