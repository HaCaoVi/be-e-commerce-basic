namespace e_commerce_basic.Helpers
{
    public class PagedResult<T>
    {
        public required Meta Meta { get; set; }
        public List<T> Result { get; set; } = [];
    }

}