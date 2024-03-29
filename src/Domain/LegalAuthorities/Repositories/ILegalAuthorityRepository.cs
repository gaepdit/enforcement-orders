﻿using Enfo.Domain.LegalAuthorities.Resources;

namespace Enfo.Domain.LegalAuthorities.Repositories;

public interface ILegalAuthorityRepository : IDisposable
{
    Task<LegalAuthorityView> GetAsync(int id);
    Task<IReadOnlyList<LegalAuthorityView>> ListAsync(bool includeInactive = false);
    Task<int> CreateAsync(LegalAuthorityCommand resource);
    Task UpdateAsync(LegalAuthorityCommand resource);
    Task UpdateStatusAsync(int id, bool newActiveStatus);
    Task<bool> ExistsAsync(int id);
    Task<bool> NameExistsAsync(string name);
}
