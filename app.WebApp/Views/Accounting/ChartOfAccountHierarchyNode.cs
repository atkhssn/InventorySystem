namespace app.WebApp.Views.Accounting
{
    public sealed class ChartOfAccountHierarchyNode
    {
        public string id { get; set; }
        public string text { get; set; }
        public List<ChartOfAccountHierarchyNode> children { get; set; } = new List<ChartOfAccountHierarchyNode>();
    }
}
