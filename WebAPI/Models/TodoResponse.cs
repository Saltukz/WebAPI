namespace WebAPI.Models
{
    public class TodoResponse
    {
      
        public int Id { get; set; }
     
        public string Text { get; set; }
      
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public string IsCompletedStr { get
            {
                return IsCompleted ? "Tamamlandı" : "Tamamlanmadı";
            } }
    }


}
