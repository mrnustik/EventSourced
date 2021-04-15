namespace EventSourced.Sample.Warehouse.API.Requests.WarehouseItem
{
    public class UpdateWarehouseItemRequest
    {
        public string Title { get; }

        public UpdateWarehouseItemRequest(string title)
        {
            Title = title;
        }
    }
}