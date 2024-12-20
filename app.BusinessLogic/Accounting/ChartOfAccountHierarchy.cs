namespace app.Services.Accounting
{
    public sealed class ChartOfAccountHierarchy
    {
        public string id { get; set; }
        public string text { get; set; }
        public List<ChartOfAccountHierarchy> children { get; set; } = new List<ChartOfAccountHierarchy>();
    }
}
