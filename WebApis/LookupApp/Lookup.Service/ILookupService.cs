using Lookup.Service.Models;

namespace Lookup.Service;

public interface ILookupService
{
    LookupResponse GetStateByZipCode(string zipCode);
}