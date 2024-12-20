namespace app.Services.Accounting
{
    public sealed class CoATypesViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public List<CoATypesViewModel> CoATypesViewModels { get; set; }
        public ResponseViewModel ResponseViewModel { get; set; }
    }
}
