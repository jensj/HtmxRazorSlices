using HtmxRazorSlices.Domain;

namespace HtmxRazorSlices.Features.ToDoFeature.Models
{
    public class ListModel
    {
        public string? Filter{ get; set; }
        public IEnumerable<ViewToDo> ToDos { get; set; }
    }
}
