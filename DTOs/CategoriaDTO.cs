namespace ApiSeries.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime FechaCreacion { get; set; }
        public List<TipoDTO> Tipos { get; set; }
    }
}
