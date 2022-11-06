namespace ApiSeries.Entidades
{
    public class SerieCategoria
    {
        public int SerieId { get; set; }
        public int CategoriaId { get; set; }
        public int Orden { get; set; }
        public Serie Serie { get; set; }
        public Categoria Categoria { get; set; }
    }
}
