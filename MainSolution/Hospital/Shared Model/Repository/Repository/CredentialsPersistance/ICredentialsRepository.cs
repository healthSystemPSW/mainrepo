using Model;
using System;

namespace Repository.CredentialsPersistance
{
   public interface ICredentialsRepository : IRepository<string, Credentials>
   {
        bool CreateIfUnique(Credentials newValue);
   }
}