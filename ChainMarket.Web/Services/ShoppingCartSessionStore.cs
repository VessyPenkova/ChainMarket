
using System.Text.Json;
using ChainMarket.Web.Models;
using Microsoft.AspNetCore.Http;

namespace ChainMarket.Web.Services
{
    public class ShoppingCartSessionStore
    {
        private const string SessionKey = "ShoppingCart";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartSessionStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<ShoppingCartItem> GetItems()
        {
            var session = GetSession();
            var json = session.GetString(SessionKey);

            if (string.IsNullOrEmpty(json))
                return new List<ShoppingCartItem>();

            return JsonSerializer.Deserialize<List<ShoppingCartItem>>(json) ?? new List<ShoppingCartItem>();
        }

        public void SaveItems(List<ShoppingCartItem> items)
        {
            var session = GetSession();
            var json = JsonSerializer.Serialize(items);

            session.SetString(SessionKey, json);
        }

        private ISession GetSession()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
                throw new InvalidOperationException("HTTP context is not available.");

            return context.Session;
        }
    }
}
