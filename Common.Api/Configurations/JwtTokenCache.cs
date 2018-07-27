using System.Collections.Generic;

namespace Common360.Api.Configurations
{
    public class JwtTokenCache
    {
        public ICollection<string> Tokens { get; set; } = new HashSet<string>();

        public void Add(string token)
        {
            Tokens.Add(token);
        }
        internal void Remove(string tokenId)
            => Tokens.Remove(tokenId);

        public bool Contains(string id)
            => Tokens.Contains(id);
    }
}
