namespace ToDoList.Domain.Response
{
    public class DataTableResult
    { // Для возвращения данных и пагинации
        public object Data { get; set; }
        public int Total { get; set; }
    }
}
