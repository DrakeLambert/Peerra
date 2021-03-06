using System.ComponentModel.DataAnnotations;

namespace DrakeLambert.Peerra.ViewModels
{
    public class NewTopic
    {
        [Required, StringLength(50)]
        public string Title { get; set; }

        [Required, StringLength(200)]
        public string Description { get; set; }

        [Required]
        public bool IsNotLeaf { get; set; }
    }
}
