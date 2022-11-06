namespace ApiSeries.DTOs
{
    public class CategoriaDTOConSeries : CategoriaDTO
    {
        public List<GetSerieDTO> Series { get; set; }
    }
}
