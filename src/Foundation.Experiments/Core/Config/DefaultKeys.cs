namespace Foundation.Experiments.Core.Config
{
    public class DefaultKeys
    {
        public const string CookieName = "epiOptimization";
        public const string VisitorGroup = "VisitorGroup";
        public const string UserRoles = "UserRole";
        public const string UserLoggedIn = "UserLoggedIn";

        // Event  tags
        public const string Items = "Products in the basket/purchase order";
        public const string OneProductInOrder = "One product in order";
        public const string TwoThreeProductInOrder = "Two or three products in order";
        public const string FourOrMoreProductInOrder = "Four or more products in order";
        public const string Currency = "Currency";
        public const string Language = "Language";
        public const string Channel = "Channel";
        public const string ParentPageUri = "Parent page uri";
        public const string CategoryName = "Category";
        public const string Revenue = "revenue";
        public const string PageType = "PageType";

        // Event types
        public const string EventBasket = "basket";
        public const string EventCategory = "category";
        public const string EventOrder = "order";
        public const string EventProduct = "product";
        public const string EventPageView = "epiPageView";
        public const string EventWishlist = "wishlist";
        public const string EventRevenue = "orderValue";
    }
}
